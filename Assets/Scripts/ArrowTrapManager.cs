using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapManager : MonoBehaviour
{
    [SerializeField] List<GameObject> arrows; // This list shoud be oredered correctly.
    List<Vector3> backupPos = new List<Vector3>();//For reseting poses.
    Vector3Int playerTileMapPos;

    private void Start()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            backupPos.Add(arrows[i].transform.position);
        }
    }

    /// <summary>
    /// Get a backup from player position.
    /// </summary>
    /// <param name="playerTilePos"></param>
    public void SetPlayerTilePosRef(Vector3Int playerTilePos)
    {
        playerTileMapPos = playerTilePos;
    }

    /// <summary>
    /// Return an arrow from all 6 arrows that face the player.
    /// </summary>
    /// <returns></returns>
    public GameObject GetCorrectArrow()
    {
        return arrows[playerTileMapPos.x]; // Example: arrows[0] is for tile with X = 0
    }

    /// <summary>
    /// Set arrow back to it's position
    /// </summary>
    public void ResetPosition()
    {
        arrows[playerTileMapPos.x].transform.position = backupPos[playerTileMapPos.x];
    }

    /// <summary>
    /// Get average velocity (difference of goal position and first position divided by difference of begin time and final time)
    /// playerPos is goal pos and total length is final time.
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="totalLength"></param>
    /// <returns></returns>

    public float GetAverageVelocity(Vector3 playerPos,float totalLength)
    {
        float startY = arrows[playerTileMapPos.x].transform.position.y;
        float deltaY = playerPos.y - startY;
        return Mathf.Abs(deltaY / totalLength);

    }
}