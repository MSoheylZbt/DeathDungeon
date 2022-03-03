using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapManager : MonoBehaviour
{
    [SerializeField] List<GameObject> arrows;
    List<Vector3> backupPos = new List<Vector3>();
    Vector3Int playerTileMapPos;

    private void Start()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            backupPos.Add(arrows[i].transform.position);
        }
    }

    public void SetPlayerTilePos(Vector3Int playerTilePos)
    {
        playerTileMapPos = playerTilePos;
    }

    public GameObject GetCorrectArrow()
    {
        return arrows[playerTileMapPos.x]; // EX: arrows[0] is for tile with X = 0
    }

    public void ResetPosition()
    {
        arrows[playerTileMapPos.x].transform.position = backupPos[playerTileMapPos.x];
    }

    public float GetAverageVelocity(Vector3 playerPos,float totalLength)
    {
        float startY = arrows[playerTileMapPos.x].transform.position.y;
        float deltaY = Mathf.Abs(playerPos.y) - Mathf.Abs(startY);
        print("Average Speed is: " + (deltaY / totalLength).ToString());
        return Mathf.Abs(deltaY / totalLength);

    }
}
