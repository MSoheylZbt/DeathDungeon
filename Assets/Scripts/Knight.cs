using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    public static Knight instance;

    [SerializeField] Knight_Data data;
    [SerializeField] Sounds_data sounds;
    [SerializeField] Coin coinPref;

    public delegate void Moving();
    public static event Moving OnMove;

    #region Cache
    Vector2 moveAmount = new Vector2();
    Tilemap tilemap;
    Animator animator;
    GridHandler gridHandler;
    ArrowReact arrowReact;
    FireReact fireReact;
    ResetGame gameReseter;
    Coin coin;
    AudioSource audioSource;
    Vector3 playerFirePos;
    #endregion

    bool isFreezed = false;

    private void Awake()
    {
        //print("Kepp Knight Awake");
        KeepKnight();
    }

    private void KeepKnight()
    {
        if(instance != null)
        {
            //print("<color=yellow> Knight is detroying </color>");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() //Can't call in Awake because Knight_Data has OnEnable and OnEnable calls after Awake
    {
        //print("Knight Called");
        data.ResetData();
    }

    public void Init(GridHandler handler, ReactManager reactManager,ResetGame reset)
    {
        //print("Knight Init");
        data.playerFirstPos = transform.position;

        gridHandler = handler;
        animator = GetComponent<Animator>();
        tilemap = gridHandler.GetTileMap();
        moveAmount.x = tilemap.cellSize.x;
        moveAmount.y = tilemap.cellSize.y;

        arrowReact = reactManager.arrowReact;
        fireReact = reactManager.fireReact;

        gameReseter = reset;

        audioSource = GetComponent<AudioSource>();
    }

    public void Init()
    {
        animator = GetComponent<Animator>();
        moveAmount = data.moveAmount;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isFreezed)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovingCheck(KeyCode.UpArrow);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovingCheck(KeyCode.DownArrow);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Left", true);
            MovingCheck(KeyCode.LeftArrow);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("Left", false);
            MovingCheck(KeyCode.RightArrow);
        }
    }

    private void MovingCheck(KeyCode pressedKey)
    {
        Vector3 newPos = new Vector3();
        Vector3 rayDir = new Vector3();
        float cellXY = 0f;

        switch (pressedKey)
        {
            case KeyCode.UpArrow:
                newPos = new Vector3(transform.position.x, transform.position.y + moveAmount.y, transform.position.z);
                rayDir = transform.up;
                cellXY = moveAmount.y;
                break;

            case KeyCode.DownArrow:
                newPos = new Vector3(transform.position.x, transform.position.y - moveAmount.y, transform.position.z);
                rayDir = -transform.up;
                cellXY = moveAmount.y;
                break;

            case KeyCode.LeftArrow:
                newPos = new Vector3(transform.position.x - moveAmount.x, transform.position.y, transform.position.z);
                rayDir = -transform.right;
                cellXY = moveAmount.x;
                break;

            case KeyCode.RightArrow:
                newPos = new Vector3(transform.position.x + moveAmount.x, transform.position.y, transform.position.z);
                rayDir = transform.right;
                cellXY = moveAmount.x;
                break;
        }

        if (isBlock(newPos,rayDir,cellXY) == false)
        {
            audioSource.PlayOneShot(sounds.walkSound,sounds.walkAmount);
            transform.position = newPos;
            coin?.Disable();
            OnMove?.Invoke();
            if (gridHandler)
            {
                //print("Grid Handling");
                TileContentCheck(newPos);
            }
        }

    }

    //TODO :: REFACTOR
    private void TileContentCheck(Vector3 newPos)
    {
        //print("Checking Contents");
        int coinsAmount = 0;

        if (gridHandler.isSteppedOnTreasure(newPos,out coinsAmount))
        {
            AddCoins(coinsAmount);

        }
        else if (gridHandler.isSteppedOnFireTrap(newPos))
        {
            audioSource.PlayOneShot(sounds.fireSound, sounds.fireAmount);
            if (data.InvisPotionCount > 0)
            {
                OnMove += SetFireTile;
                playerFirePos = transform.position;
                data.InvisPotionCount--;
            }
            else
            {
                //Debug.Log("Call from " + "<color=black> Knight: </color>" + "<color=orange> Fire Activated </color>");
                fireReact.StartFireTimer(GetReductionTime());
            }

        } else if(gridHandler.isSteppedOnArrowTrap(newPos))
        {
            audioSource.PlayOneShot(sounds.arrowSound,sounds.arrowAmount);
            if(data.InvisPotionCount > 0)
                data.InvisPotionCount--;
            else
            {
                SetFreeze(true);
                arrowReact.StartArrowTimer(GetReductionTime());
            }
        } 
        else
        {
            gridHandler.CheckForFinalDoor(transform.position);
        }
    }

    private void PlayCoinAnimation()
    {
        if (!coin)
        {
            coin = Instantiate(coinPref, transform.position, Quaternion.identity, this.transform);
        }
        else
        {
            coin.transform.position = transform.position;
            coin.Enable();
        }
        coin.PlayAnimation();
    }

    private void SetFireTile() // Just For UnSubscribing Event
    {
        fireReact.SetTileFireImage(playerFirePos);
        OnMove -= SetFireTile;
    }

    private float GetReductionTime()
    {
        if (data.GreePotionCount > 0)
        {
            data.GreePotionCount--;
            return data.greenTimeReduction;
        }
        else
            return 0;
    }

    private bool isBlock(Vector3 newPos,Vector3 dir,float rayDistance)
    {
        RaycastHit2D hitted = Physics2D.Raycast(transform.position, dir,rayDistance);
        //Debug.DrawRay(transform.position, dir, Color.red, 1f);

        return hitted;
    }

    public void TakeDamage()
    {
        //print("<color=red> Damage taken! </color>");
        animator.SetTrigger("GetHit");
        audioSource.PlayOneShot(sounds.damageSound,sounds.damageAmount);
        if(data.HealthPotionCount > 0)
        {
            data.HealthPotionCount--;
            ResetStrike();
            return;
        }
        else
        {
            data.Health--;
            if (data.Health == 0)
            {
                //print("<color=red> Dying! </color>");
                Die();
            }
        }
    }

    public void AddCoins(int coinsAmount)
    {
        //Debug.Log(coinsAmount + " <color=yellow> Coins Adeed! </color> ");
        data.Coins += coinsAmount;
        PlayCoinAnimation();
        audioSource.PlayOneShot(sounds.coinSound,sounds.coinAmount);
    }

    public void Die()
    {
        gameReseter.ShowMenu();
        gameObject.SetActive(false);
    }


    public Vector3Int GetPlayerTilePos()
    {
        return (tilemap.WorldToCell(transform.position) - tilemap.origin);
    }

    public Tilemap GetTileMap()
    {
        return tilemap;
    }

    public void SetFreeze(bool value)
    {
        isFreezed = value;
    }

    public void SetMoveAmount(Vector2 amount)
    {
        moveAmount = amount;
    }

    public Vector3 GetPlayerFirstPos()
    {
        return data.playerFirstPos;
    }

    public void PlayGreenAnimation()
    {
        //print("<color=green> Green Animation played! </color>");
        audioSource.PlayOneShot(sounds.shieldSound,sounds.shieldAmount);
        animator.SetTrigger("GreenShield");
    }

    public void PlayYellowAnimation()
    {
        //print("<color=yellow> Yellow Animation played! </color>");
        audioSource.PlayOneShot(sounds.shieldSound,sounds.shieldAmount);
        animator.SetTrigger("YellowShield");
    }

    public void SetScore(int score)
    {
        //print("Set Score " + score);
        data.Score = score;
    }

    public void ResetStrike()
    {
        data.Strike = 1;
    }

    public void IncrementStrike(ReflexUI reflexUI)
    {
        data.Strike++;
        reflexUI.SetSliderText(data.Strike.ToString());
    }
}
