using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GridHandler : MonoBehaviour
{

    [SerializeField] SO_Grid gridData;
    [SerializeField] int allowedTreasureRow;
    int gridSize;

    #region Cache
    Tilemap tilemap;
    List<Vector3Int> trapPoses = new List<Vector3Int>();
    List<Vector3Int> treasurePoses = new List<Vector3Int>();
    #endregion

    #region Test
    //[SerializeField] TextMeshProUGUI debugText;
    //[SerializeField] Canvas canvas;
    #endregion

    void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        gridSize = tilemap.size.x * tilemap.size.y;
        GeneratingGrid();
    }

    public void GeneratingGrid()
    {
        gridData.SetRandomDifficulty();
        GenerateTraps();
        GenerateTreasures();
    }

    private void GenerateTraps()
    {
        int trapCounts = gridData.GetTrapCount();
        int tileMapSize = tilemap.size.x;// X is equal to Y

        for (int i = 0; i < trapCounts; i++)
        {
            int randomNumber = Random.Range(0,gridSize);
            //print("Without origin:" + new Vector3Int(randomNumber % tileMapSize, randomNumber / tileMapSize, 0));
            Vector3Int gridPos = new Vector3Int(randomNumber % tileMapSize, randomNumber / tileMapSize, 0) + tilemap.origin;
            if ( randomNumber < tileMapSize || trapPoses.Contains(gridPos))
            {
                i--;
                continue;
            }
            else
            {
                trapPoses.Add(gridPos);
                //Debug.Log("Random number is : " + randomNumber + " grid pos is : " + gridPos + " Final is : " + (tilemap.origin + gridPos).ToString());
            }

            //Instantiate(debugText,tilemap.CellToWorld(tilemap.origin + gridPos),Quaternion.identity);
        }
    }

    private void GenerateTreasures()
    {
        int treasureCount = gridData.GetTreasureCount();
        int tileMapSize = tilemap.size.x;// X is equal to Y

        for (int i = 0; i < treasureCount; i++)
        {
            int randomNumber = Random.Range(0, gridSize);
            Vector3Int gridPos = new Vector3Int(randomNumber % tileMapSize, randomNumber / tileMapSize, 0) + tilemap.origin;
            if (randomNumber < (tileMapSize * allowedTreasureRow) || trapPoses.Contains(gridPos) || treasurePoses.Contains(gridPos))
            {
                i--;
                continue;
            }
            else
            {
                treasurePoses.Add(gridPos);
                tilemap.SetTile(gridPos, gridData.treasureTile);
                //Debug.Log("Random number is : " + randomNumber + " grid pos is : " + gridPos + " Final is : " + (tilemap.origin + gridPos).ToString());
            }

            //Instantiate(debugText,tilemap.CellToWorld(tilemap.origin + gridPos),Quaternion.identity);
        }

    }

    public bool CheckForTrap(Vector3 tileWorldPos)
    {
        Vector3Int cellGridPos = tilemap.WorldToCell(tileWorldPos);
        if(trapPoses.Contains(cellGridPos))
        {
            tilemap.SetTile(cellGridPos, gridData.trapTile);
            trapPoses.Remove(cellGridPos);
            return true;
        }
        else
        {

            return false;
        }
    }

    public bool CheckForTreasure(Vector3 tileWorldPos)
    {
        Vector3Int cellGridPos = tilemap.WorldToCell(tileWorldPos);
        if (treasurePoses.Contains(cellGridPos))
        {
            tilemap.SetTile(cellGridPos, gridData.openedTreasureTile);
            treasurePoses.Remove(cellGridPos);
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
