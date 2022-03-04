using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Knight : MonoBehaviour
{
    [SerializeField] GridHandler gridHandler;

    int health = 3;

    #region Cache
    Tilemap tilemap;
    ArrowReact arrowReact;
    FireReact fireReact;
    #endregion


    private void Start()
    {
        tilemap = gridHandler.GetTileMap();


        arrowReact = GetComponent<ArrowReact>();
        arrowReact.Init(this);

        fireReact = GetComponent<FireReact>();
        fireReact.Init(this);
    }

    private void Update()
    {
        if (arrowReact.GetCurrentState() != TimerState.NotStarted)
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
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x + tilemap.cellSize.x, transform.position.y, transform.position.z);
            if (isBlock(newPos,transform.right,tilemap.cellSize.x) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }

        }
    }

    private void TileContentCheck(Vector3 newPos)
    {
        if (gridHandler.isSteppedOnTreasure(newPos))
        {
            //Debug.Log("<color=red> Trapped! </color> ");
        }
        else if (gridHandler.isSteppedOnFireTrap(newPos))
        {
            fireReact.StartFireTimer();
            //Debug.Log("<color=yellow> Treasure! </color> ");
        } else if(gridHandler.isSteppedOnArrowTrap(newPos))
        {
            arrowReact.StartArrowTimer();
        }
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
        health--;
        if(health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("Die Motherfucker");
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
}
