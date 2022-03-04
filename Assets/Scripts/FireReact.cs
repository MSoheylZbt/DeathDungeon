using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireReact : React
{
    [Header("From FireReact")]
    [SerializeField] TileBase fireTile;
    [SerializeField] float yellowStart;
    [SerializeField] float greenStart;
    [SerializeField] float totalLength;



    private void Update()
    {
        if (GetCurrentState() != TimerState.NotStarted)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                base.ShowReaction();
                SetTileFireImage();
            }
        }

    }

    public void StartFireTimer()
    {
        OnTimerEnd += KillWithFireTrap;
        base.StartTimer(greenStart,yellowStart,totalLength);
    }

    private void KillWithFireTrap()
    {
        SetTileFireImage();
        player.Die();
    }

    private void SetTileFireImage()
    {
        Tilemap tilemap = player.GetTileMap();
        tilemap.SetTile(tilemap.WorldToCell(player.transform.position), fireTile);

        OnTimerEnd -= KillWithFireTrap;
    }
}
