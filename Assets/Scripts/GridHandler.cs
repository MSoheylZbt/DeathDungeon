using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GridHandler : MonoBehaviour
{

    [SerializeField] GridGeneratingData gridData;
    [SerializeField] int allowedTreasureRow; //From which row Treasures can be spawned?

    #region Cache
    LevelManager levelManager;
    //Tilemap is a built-in greed based tools that helps us to build a grid based world in unity
    Tilemap tilemap;
    int gridSize;
    int tileMapLength;
    GameObject coin;
    #endregion

    //every tile that has a content on it will be added to theri specified list.
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
        InitAvailablePoses();
        GeneratingGrid();
    }

    /// <summary>
    /// Init Grid related params.
    /// </summary>
    private void InitGrid()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        gridSize = tilemap.size.x * tilemap.size.y;
        tileMapLength = tilemap.size.x;
    }

    private void InitAvailablePoses()
    {
        for (int i = 0; i < gridSize; i++)
        {
            Vector3Int gridPos = new Vector3Int(i % tileMapLength, i / tileMapLength, 0) + tilemap.origin; // X is equal to Y
            availablePoses.Add(gridPos);
        }
    }


    private void GeneratingGrid()
    {
        gridData.SetRandomDifficulty();
        GenerateTilesContent(gridData.GetTreasureCount(), allowedTreasureRow, treasurePoses,gridData.treasureTile);
        GenerateTilesContent(gridData.GetFireTrapCounts(),1,fireTrapPoses);
        GenerateTilesContent(gridData.GetArrowTrapCounts(),1,arrowTrapPoses);
    }

    /// <summary>
    /// Generate random number from available grid poses indexes and set it as given TilesContent.
    /// and set a image for it.
    /// </summary>
    /// <param name="trapCounts"></param>
    /// <param name="allowedRow"></param>
    /// <param name="trapList"></param>
    /// <param name="imgTile"></param>
    private void GenerateTilesContent(int trapCounts,int allowedRow,List<Vector3Int> trapList,TileBase imgTile)
    {
        for (int i = 0; i < trapCounts; i++)
        {
            //Randomly choose a available position.
            int randomNumber = Random.Range(0, availablePoses.Count);
            Vector3Int gridPos = availablePoses[randomNumber];

            if (randomNumber < (tileMapLength * allowedRow)) // each row ended with length of tile map factors.
            {
                i--;
                continue;
            }
            else
            {
                trapList.Add(gridPos);
                availablePoses.Remove(gridPos);
                tilemap.SetTile(gridPos, imgTile);
            }
        }


    }

    /// <summary>
    /// Generate random number from available grid poses indexes and set it as given TilesContent.
    /// </summary>
    /// <param name="trapCounts"></param>
    /// <param name="allowedRow"></param>
    /// <param name="trapList"></param>
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

    /// <summary>
    /// Check if we reach to the final door.
    /// </summary>
    /// <param name="newPos"></param>
    public void CheckForFinalDoor(Vector3 newPos)
    {
        levelManager.CheckForLevelFinal(newPos);
    }

    /// <summary>
    /// returns true if player steps on a Fire trap.
    /// </summary>
    /// <param name="tileWorldPos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// returns true if player steps on a Treasure.
    /// </summary>
    /// <param name="tileWorldPos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// returns true if player steps on a Arrow trap.
    /// </summary>
    /// <param name="tileWorldPos"></param>
    /// <returns></returns>
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
