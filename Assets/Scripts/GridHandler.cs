using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GridHandler : MonoBehaviour
{

    [SerializeField] SO_Grid gridData;
    [SerializeField] int allowedTreasureRow;

    #region Cache
    LevelManager levelManager;
    Tilemap tilemap;
    int gridSize;
    int tileMapLength;
    GameObject coin;
    #endregion

    #region TilesContents
    List<Vector3Int> fireTrapPoses = new List<Vector3Int>();
    List<Vector3Int> arrowTrapPoses = new List<Vector3Int>();
    List<Vector3Int> treasurePoses = new List<Vector3Int>();
    List<Vector3Int> availablePoses = new List<Vector3Int>();
    #endregion

    public void Init(LevelManager lvlManager)
    {
        levelManager = lvlManager;
        InitGrid();
        GeneratingGrid();
    }

    private void InitGrid()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        gridSize = tilemap.size.x * tilemap.size.y;
        tileMapLength = tilemap.size.x;
    }

    private void GeneratingGrid()
    {
        gridData.SetRandomDifficulty();
        InitAvailablePoses();
        GenerateTilesContent(gridData.GetTreasureCount(), allowedTreasureRow, treasurePoses,gridData.treasureTile);
        GenerateTilesContent(gridData.GetFireTrapCounts(),1,fireTrapPoses);
        GenerateTilesContent(gridData.GetArrowTrapCounts(),1,arrowTrapPoses);
    }

    private void InitAvailablePoses()
    {
        for (int i = 0; i < gridSize; i++)
        {
            Vector3Int gridPos = new Vector3Int(i % tileMapLength, i / tileMapLength, 0) + tilemap.origin; // X is equal to Y
            availablePoses.Add(gridPos);
        }
    }

    private void GenerateTilesContent(int trapCounts,int allowedRow,List<Vector3Int> trapList,TileBase imgTile)
    {
        for (int i = 0; i < trapCounts; i++)
        {
            int randomNumber = Random.Range(0, availablePoses.Count);
            Vector3Int gridPos = availablePoses[randomNumber];
            //print("Without origin:" + new Vector3Int(randomNumber % tileMapSize, randomNumber / tileMapSize, 0));

            if (randomNumber < (tileMapLength * allowedRow))
            {
                i--;
                continue;
            }
            else
            {
                trapList.Add(gridPos);
                availablePoses.Remove(gridPos);
                tilemap.SetTile(gridPos, imgTile);
                //Debug.Log("Random number is : " + randomNumber + " grid pos is : " + gridPos + " Final is : " + (tilemap.origin + gridPos).ToString());
            }

            //Instantiate(debugText,tilemap.CellToWorld(tilemap.origin + gridPos),Quaternion.identity);
        }


    }

    private void GenerateTilesContent(int trapCounts, int allowedRow, List<Vector3Int> trapList)
    {
        for (int i = 0; i < trapCounts; i++)
        {
            int randomNumber = Random.Range(0, availablePoses.Count);
            Vector3Int gridPos = availablePoses[randomNumber];

            if (randomNumber < (tileMapLength * allowedRow))
            {
                i--;
                continue;
            }
            else
            {
                trapList.Add(gridPos);
                availablePoses.Remove(gridPos);
            }
        }
    }

    public void CheckForFinalDoor(Vector3 newPos)
    {
        levelManager.CheckForLevelFinal(newPos);
    }

    public bool isSteppedOnFireTrap(Vector3 tileWorldPos)
    {
        Vector3Int cellGridPos = tilemap.WorldToCell(tileWorldPos);
        if(fireTrapPoses.Contains(cellGridPos))
        {
            fireTrapPoses.Remove(cellGridPos);
            return true;
        }
        else
            return false;
    }

    public bool isSteppedOnTreasure(Vector3 tileWorldPos, out int coinsAmount)
    {
        Vector3Int cellGridPos = tilemap.WorldToCell(tileWorldPos);
        if (treasurePoses.Contains(cellGridPos))
        {
            tilemap.SetTile(cellGridPos, gridData.openedTreasureTile);
            treasurePoses.Remove(cellGridPos);
            coinsAmount = gridData.GetTreasureCoins();
            return true;
        }
        else
        {
            coinsAmount = 0;
            return false;
        }
    }

    public bool isSteppedOnArrowTrap(Vector3 tileWorldPos)
    {
        Vector3Int cellGridPos = tilemap.WorldToCell(tileWorldPos);
        if (arrowTrapPoses.Contains(cellGridPos))
        {
            tilemap.SetTile(cellGridPos, gridData.arrowTrapTile);
            arrowTrapPoses.Remove(cellGridPos);
            return true;
        }
        else
            return false;
    }

    public Tilemap GetTileMap()
    {
        return tilemap;
    }

}
