using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int shopLevelIndex = 4;

    #region Cache
    static int levelIndex = 0; // Is static for remaining during level transition.

    Tilemap tilemap;
    Animator animator;

    List<Vector3Int> doorWorldPos = new List<Vector3Int>();
    #endregion
    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        animator = GetComponent<Animator>();
        SetAllTiles();
    }

    private void SetAllTiles()
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
        Knight.instance.transform.position = Knight.instance.GetPlayerFirstPos();

        levelIndex++;
        //print("level index: " + levelIndex);
        if (levelIndex == shopLevelIndex)
        {
            levelIndex = 0;
            Knight.instance.SetMoveAmount(new Vector2(tilemap.cellSize.x, tilemap.cellSize.y));
            SceneManager.LoadScene(1);
        }
        else
            SceneManager.LoadScene(0);

        animator.SetBool("OpenDoor", false);
        Knight.instance.gameObject.SetActive(true);

    }

    public void LoadLevelFromShop()
    {
        Knight.instance.transform.position = Knight.instance.GetPlayerFirstPos();
        SceneManager.LoadScene(0);
    }

    public void ResetLevelIndex()
    {
        levelIndex = 0;
    }
}
