using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Knight : MonoBehaviour
{
    [SerializeField] GridHandler gridHandler;
    [SerializeField] Knight_Data data;

    #region Cache
    [SerializeField] GameObject reactManager;
    Vector2 moveAmount = new Vector2();
    Tilemap tilemap;
    Animator animator;
    ArrowReact arrowReact;
    FireReact fireReact;
    #endregion

    bool isFreezed = false;

    private void Awake()
    {
        KeepKnight();
    }

    private void KeepKnight()
    {
        DontDestroyOnLoad(this);
        Knight[] knights = FindObjectsOfType<Knight>();
        if (knights.Length > 1)
        {
            Destroy(knights[1].gameObject);
        }
    }

    private void Start()
    {
        print("Start");
        InitKnightFunctions();
    }

    public void InitKnightFunctions()
    {
        data.ResetData();
        animator = GetComponent<Animator>();
        print("Knight Init");

        if (gridHandler)
        {
            print("Grid handler set ");
            tilemap = gridHandler.GetTileMap();
            moveAmount.x = tilemap.cellSize.x;
            moveAmount.y = tilemap.cellSize.y;
            arrowReact = reactManager.GetComponent<ArrowReact>();
            fireReact = reactManager.GetComponent<FireReact>();
            arrowReact.Init(this);
            fireReact.Init(this);
        }
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
            transform.position = newPos;
            if (gridHandler)
            {
                print("Grid Handling");
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
            //Debug.Log("<color=red> Trapped! </color> ");
        }
        else if (gridHandler.isSteppedOnFireTrap(newPos))
        {
            if (data.invisiblePotionCount > 0)
                data.invisiblePotionCount--;
            else
                fireReact.StartFireTimer(GetReductionTime());

        } else if(gridHandler.isSteppedOnArrowTrap(newPos))
        {
            if(data.invisiblePotionCount > 0)
                data.invisiblePotionCount--;
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

    private float GetReductionTime()
    {
        if (data.greenTimePotionCount > 0)
        {
            data.greenTimePotionCount--;
            return data.greenTimeReduction;
        }
        else
            return 0;
    }

    private bool isBlock(Vector3 newPos,Vector3 dir,float rayDistance)
    {
        RaycastHit2D hitted = Physics2D.Raycast(transform.position, dir,rayDistance);
        Debug.DrawRay(transform.position, dir, Color.red, 1f);

        return hitted;
    }

    public void TakeDamage()
    {
        print("<color=red> Damage taken! </color>");

        if(data.healthPotionCount > 0)
        {
            data.healthPotionCount--;
            //TODO :: Animation :: Removie a heart then use a potion.
            return;
        }
        else
        {
            data.currentHealth--;
            if (data.currentHealth == 0)
            {
                Die();
            }
        }
    }

    public void AddCoins(int coinsAmount)
    {
        data.currentCoins += coinsAmount;
    }

    public void Die()
    {
        print("Die Motherfucker");
        gameObject.SetActive(false);
    }

    public void UpgradeArmor()
    {
        data.maxHealth++;
    }

    public void BuyHealthPotion(int price)
    {
        data.currentCoins -= price;
        if(data.currentHealth == data.maxHealth)
            data.healthPotionCount++;
        else
           data.currentHealth++;
    }

    public void BuyInvisiblePotion(int price)
    {
        data.currentCoins -= price;
        data.invisiblePotionCount++;
    }

    public void BuyGreenTimePotionCount(int price)
    {
        data.currentCoins -= price;
        data.greenTimePotionCount++;
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
}
