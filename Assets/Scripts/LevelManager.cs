using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    #region cache
    static int levelIndex = 0; // Is static for remaining during level transition.
    Tilemap tilemap;
    Animator animator;
    Vector3 playerFirstPos = new Vector3();
    #endregion

    [SerializeField] int shopLevelIndex = 4;
    [SerializeField] Knight knight;
    List<Vector3Int> doorWorldPos = new List<Vector3Int>();

    public void Init()
    {
        playerFirstPos = knight.transform.position;
        //print(playerFirstPos);
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
            knight.gameObject.SetActive(false);
        }
    }

    public void LoadLevel()//Calling from animation event
    {
        knight.transform.position = playerFirstPos;

        levelIndex++;
        print("level index: " + levelIndex);
        if (levelIndex == shopLevelIndex)
        {
            levelIndex = 0;
            knight.SetMoveAmount(new Vector2(tilemap.cellSize.x, tilemap.cellSize.y));
            SceneManager.LoadScene(1);
        }
        else
            SceneManager.LoadScene(0);

        animator.SetBool("OpenDoor", false);
        knight.gameObject.SetActive(true);

    }
}
