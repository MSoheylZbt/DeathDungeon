using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int shopLevelIndex = 4; // every 4 times that palyer beat main level, reaches a shop.
    [SerializeField] GridHandler gridPref;

    #region Cache
    static int levelIndex = 0; // Is static for remaining during level transition.

    GridHandler gridHandler;
    Tilemap tilemap;
    Animator animator;

    List<Vector3Int> doorWorldPos = new List<Vector3Int>();
    #endregion
    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        animator = GetComponent<Animator>();
        SetDoorGridPoses();
    }

    private void SetDoorGridPoses()
    {
        int size = tilemap.size.x * tilemap.size.y;
        for (int i = 0; i < size; i++)
        {
            Vector3Int gridPos = new Vector3Int(i % tilemap.size.x, i / tilemap.size.x, 0) + tilemap.origin; // X is equal to Y
            Vector3Int posWithoutOffset = new Vector3Int(gridPos.x + (int)tilemap.orientationMatrix.m03, gridPos.y + (int)tilemap.orientationMatrix.m13, 0);
            doorWorldPos.Add(posWithoutOffset);
        }
    }

    public void CheckForLevelFinal(Vector3 playerPos)
    {
        if(doorWorldPos.Contains(tilemap.WorldToCell(playerPos)))
        {
            //print("Level End");
            animator.SetBool("OpenDoor",true);
            Knight.instance.gameObject.SetActive(false);
        }
    }

    public void LoadLevel()//Calling from animation event
    {
        Knight.instance.transform.position = Knight.instance.GetPlayerFirstPos(); //reset player position
        if (gridHandler)
        {
            Destroy(gridHandler.gameObject);
        }

        levelIndex++;

        if (levelIndex == shopLevelIndex) // if player reaches the shop
        {
            levelIndex = 0;
            Knight.instance.SetMoveAmount(new Vector2(tilemap.cellSize.x, tilemap.cellSize.y)); // because there is no gridHandler in shop level, we need to SetMoveAmount manually here.
            SceneManager.LoadScene(2);
        }
        else
        {
            gridHandler = Instantiate(gridPref, new Vector2(0, 0), Quaternion.identity);
            gridHandler.Init(this);
            Knight.instance.Init(gridHandler);
            //SceneManager.LoadScene(1);
        }

        animator.SetBool("OpenDoor", false);
        Knight.instance.gameObject.SetActive(true);

    }

    public void LoadFirstLevel() // Calls from clicking on button in shop scene
    {
        Knight.instance.transform.position = Knight.instance.GetPlayerFirstPos();
        Knight.instance.gameObject.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void ResetLevelIndex()
    {
        levelIndex = 0;
    }

    public void SetGridHandler(GridHandler grid)
    {
        gridHandler = grid;
    }
}
