using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    //Used for implementing singelton pattern.
    public static Knight instance;

    [SerializeField] Knight_Data data;
    [SerializeField] Sounds_data sounds;
    [SerializeField] Coin coinPref;

    //An event that calls whenever players move.
    public delegate void Moving();
    public static event Moving OnMove;

    #region Cache
    Vector2 moveAmount = new Vector2();
    Vector3 playerFirePos;

    Tilemap tilemap;
    Animator animator;
    GridHandler gridHandler;
    ArrowReact arrowReact;
    FireReact fireReact;
    ResetGame gameReseter;
    AudioSource audioSource;
    Coin coin;
    #endregion

    bool isFreezed = false;

    //Awake Calls every time a scene loaded before any other events.
    private void Awake()
    {
        KeepKnight();
        data.ResetData();
    }

    /// <summary>
    /// When a scene loaded, a new istance of this class will be created, but with this function we kepp same instance throughout of whole game.
    /// This called singelton pattern.
    /// </summary>
    private void KeepKnight()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            print("Destroying");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Init(GridHandler handler, ReactManager reactManager,ResetGame reset)// Init for calling from Initializer
    {
        data.playerFirstPos = transform.position;

        gridHandler = handler;
        //Animator is an built-in state machine in unity that helps to play animation in correct order.
        animator = GetComponent<Animator>();
        tilemap = gridHandler.GetTileMap();

        //moveAmount is based on tileMap size
        moveAmount.x = tilemap.cellSize.x;
        moveAmount.y = tilemap.cellSize.y;

        arrowReact = reactManager.arrowReact;
        fireReact = reactManager.fireReact;

        gameReseter = reset;

        audioSource = GetComponent<AudioSource>();
    }

    public void Init() // Init for calling from Shop Scene
    {
        animator = GetComponent<Animator>();
        moveAmount = data.moveAmount;
    }

    public void Init(GridHandler temp)
    {
        gridHandler = temp;
        tilemap = gridHandler.GetTileMap();
    }

    private void Update() //Called at start of each frame
    {
        if (!isFreezed)
            Move();
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Check for Up arrow Keys
        {
            MovingCheck(KeyCode.UpArrow);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovingCheck(KeyCode.DownArrow);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Left", true); // Play knight turning left animation.
            MovingCheck(KeyCode.LeftArrow);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetBool("Left", false);
            MovingCheck(KeyCode.RightArrow);
        }
    }

    /// <summary>
    /// Based on player pressed key move player to a new position
    /// </summary>
    /// <param name="pressedKey"></param>
    private void MovingCheck(KeyCode pressedKey)
    {
        Vector3 newPos = new Vector3();
        Vector3 rayDir = new Vector3();
        float cellXY = 0f;

        switch (pressedKey)
        {
            case KeyCode.UpArrow:
                newPos = new Vector3(transform.position.x, transform.position.y + moveAmount.y, transform.position.z); // move up with y position with amount of moveAmount variable.
                rayDir = transform.up;
                cellXY = moveAmount.y;
                break;

            case KeyCode.DownArrow:
                newPos = new Vector3(transform.position.x, transform.position.y - moveAmount.y, transform.position.z);
                rayDir = -transform.up;
                cellXY = moveAmount.y;
                break;

            case KeyCode.LeftArrow:
                newPos = new Vector3(transform.position.x - moveAmount.x, transform.position.y, transform.position.z);// move Left with x position with amount of moveAmount variable.
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
            audioSource.PlayOneShot(sounds.walkSound,sounds.walkAmount); //Play Moving sound
            transform.position = newPos;//Move player

            coin?.Disable();//if coin object is not null then call disable method.
            OnMove?.Invoke();//if OnMove event is not null then Invoke it.

            if (gridHandler) // if gridhanler is null it means player is in shop scene then there is no need for ContentCheck
            {
                TileContentCheck(newPos);
            }
        }

    }

    /// <summary>
    /// Every time this function called, Player current pos is checked for having treasure or traps with use of Content Lists.
    /// </summary>
    /// <param name="newPos"></param>
    private void TileContentCheck(Vector3 newPos)
    {
        int coinsAmount = 0;
        if (gridHandler.isSteppedOnTreasure(newPos,out coinsAmount))
        {
            AddCoins(coinsAmount);

        }
        else if (gridHandler.isSteppedOnFireTrap(newPos))
        {
            audioSource.PlayOneShot(sounds.fireSound, sounds.fireAmount);
            if (data.InvisPotionCount > 0) // if player has inivisble potion then traps won't affect them.
            {
                OnMove += SetFireTile; //Add SetFireTile function as a listener to OnMove event
                playerFirePos = transform.position;
                data.InvisPotionCount--;
            }
            else
            {
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
        if (!coin) //If there is no spawned coin is scene, spawn a coin
        {
            coin = Instantiate(coinPref, transform.position, Quaternion.identity, this.transform);
        }
        else //otherwise set it's position and enable it.
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
            return 0; //Shouldn't reduct time.
    }


    /// <summary>
    /// With Raycasting check for Collider that blocks player movement
    /// </summary>
    /// <param name="newPos"></param>
    /// <param name="dir"></param>
    /// <param name="rayDistance"></param>
    /// <returns></returns>
    private bool isBlock(Vector3 newPos,Vector3 dir,float rayDistance)
    {
        //A raycast from player position to a postion in distance of rayDistance and in direction of dir is drawn.
        RaycastHit2D hitted = Physics2D.Raycast(transform.position, dir,rayDistance);

        //Only objects with colliders are Blocks so anytime hitted is true we hit and block and we can't move.
        return hitted; // if raycast hit anythin it will be return true.
    }

    /// <summary>
    /// Play Hitting animation and decrease player health
    /// </summary>
    public void TakeDamage()
    {
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
                Die();
        }
    }

    /// <summary>
    /// Player get coins
    /// </summary>
    /// <param name="coinsAmount"></param>
    public void AddCoins(int coinsAmount)
    {
        //Debug.Log(coinsAmount + " <color=yellow> Coins Adeed! </color> ");
        data.Coins += coinsAmount;
        PlayCoinAnimation();
        audioSource.PlayOneShot(sounds.coinSound,sounds.coinAmount);
    }

    /// <summary>
    /// Game over
    /// </summary>
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

    public void SetFreeze(bool value) //Player won't move
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

    public void PlayGreenShieldAnimation()
    {
        audioSource.PlayOneShot(sounds.shieldSound,sounds.shieldAmount);
        animator.SetTrigger("GreenShield");
    }

    public void PlayYellowShieldAnimation()
    {
        audioSource.PlayOneShot(sounds.shieldSound,sounds.shieldAmount);
        animator.SetTrigger("YellowShield");
    }

    public void SetScore(int score)
    {
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