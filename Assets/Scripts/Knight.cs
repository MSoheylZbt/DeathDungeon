using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Knight : MonoBehaviour
{
    [SerializeField] GridHandler gridHandler;
    [SerializeField] Knight_Data data;

    #region Cache
    [SerializeField] GameObject reactManager;
    Tilemap tilemap;
    Animator animator;
    ArrowReact arrowReact;
    FireReact fireReact;
    #endregion

    bool isFreezed = false;

    private void Start()
    {
        tilemap = gridHandler.GetTileMap();
        data.ResetData();
        animator = GetComponent<Animator>();

        arrowReact = reactManager.GetComponent<ArrowReact>();
        fireReact = reactManager.GetComponent<FireReact>();
        arrowReact.Init(this);
        fireReact.Init(this);
    }


    private void Update()
    {
        if (isFreezed)
            return;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + tilemap.cellSize.y, transform.position.z);
            if (isBlock(newPos,transform.up,tilemap.cellSize.y) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - tilemap.cellSize.y, transform.position.z);
            if (isBlock(newPos,-transform.up,tilemap.cellSize.y) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x - tilemap.cellSize.x, transform.position.y, transform.position.z);
            if (isBlock(newPos,-transform.right,tilemap.cellSize.x) == false)
            {
                animator.SetBool("Left",true);
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x + tilemap.cellSize.x, transform.position.y, transform.position.z);
            if (isBlock(newPos,transform.right,tilemap.cellSize.x) == false)
            {
                animator.SetBool("Left",false);
                transform.position = newPos;
                TileContentCheck(newPos);
            }

        }
    }

    //TODO :: REFACTOR
    private void TileContentCheck(Vector3 newPos)
    {
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
