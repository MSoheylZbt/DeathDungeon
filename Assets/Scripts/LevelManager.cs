using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    Tilemap tilemap;
    [SerializeField] TileBase[] closedDoor = new TileBase[4];
    List<Vector3Int> doorWorldPos = new List<Vector3Int>();
    Vector3Int[] doorGridPos = new Vector3Int[4];

    public void Init()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        SetAllTiles();
    }

    private void SetAllTiles()
    {
        int size = tilemap.size.x * tilemap.size.y;
        for (int i = 0; i < size; i++)
        {
            Vector3Int gridPos = new Vector3Int(i % tilemap.size.x, i / tilemap.size.x, 0) + tilemap.origin; // X is equal to Y
            doorGridPos[i] = gridPos;
            Vector3Int posWithoutOffset = new Vector3Int(gridPos.x + (int)tilemap.orientationMatrix.m03, gridPos.y + (int)tilemap.orientationMatrix.m13, 0);
            doorWorldPos.Add(posWithoutOffset);
        }
    }

    public void CheckForLevelFinal(Vector3 playerPos)
    {
        if(doorWorldPos.Contains(tilemap.WorldToCell(playerPos)))
        {
            tilemap.SetTiles(doorGridPos,closedDoor);
            print("Level End");
            //SceneManager.LoadScene(0);
        }
    }
}
