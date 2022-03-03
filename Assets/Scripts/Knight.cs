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
    Reflex reflex;
    #endregion


    private void Start()
    {
        tilemap = gridHandler.GetTileMap();
        reflex = GetComponent<Reflex>();
        reflex.Init(this);
    }

    private void Update()
    {
        if (reflex.GetCurrentState() != TimerState.NotStarted)
            return;

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + tilemap.cellSize.y, 0);
            if (isBlock(newPos,tilemap.cellSize.y) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - tilemap.cellSize.y, 0);
            if (isBlock(newPos,tilemap.cellSize.y) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x - tilemap.cellSize.x, transform.position.y, 0);
            if (isBlock(newPos,tilemap.cellSize.x) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(transform.position.x + tilemap.cellSize.x, transform.position.y, 0);
            if (isBlock(newPos,tilemap.cellSize.x) == false)
            {
                transform.position = newPos;
                TileContentCheck(newPos);
            }

        }
    }

    private void TileContentCheck(Vector3 newPos)
    {
        if (gridHandler.CheckForTrap(newPos))
        {
            reflex.StartTimer();
            //Debug.Log("<color=red> Trapped! </color> ");
        }
        else if (gridHandler.CheckForTreasure(newPos))
        {

            //Debug.Log("<color=yellow> Treasure! </color> ");
        }
    }

    private bool isBlock(Vector3 newPos,float rayDistance)
    {
        Vector3 dir = (newPos - transform.position).normalized;
        RaycastHit2D hitted = Physics2D.Raycast(transform.position, dir,rayDistance);
        //if (hitted)
        //    print(hitted.transform.gameObject.name);
        //Debug.DrawRay(transform.position, dir, Color.red, 1f);

        return hitted;
    }

    public void TakeDamage()
    {
        print("<color=red> Damage taken! </color>");
        health--;
        if(health == 0)
        {
            print("Die Motherfucker");
        }
    }

    public Vector3Int GetPlayerTilePos()
    {
        return (tilemap.WorldToCell(transform.position) - tilemap.origin);
    }
}
