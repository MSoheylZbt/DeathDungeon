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

    Vector3 playerFirstPos = new Vector3();

    private void Update()
    {
        if (GetCurrentState() == TimerState.Green || GetCurrentState() == TimerState.Yellow)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShowReaction();
            }
        }

    }

    protected override void ShowReaction()
    {
        base.ShowReaction();
        SetTileFireImage();
    }

    public void StartFireTimer()
    {
        playerFirstPos = player.transform.position;
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
        tilemap.SetTile(tilemap.WorldToCell(playerFirstPos), fireTile);
    }

    protected override void TimerEnd()
    {
        KillWithFireTrap();
        //base.TimerEnd();
    }

    protected override void RedState()
    {
        player.SetFreeze(true);
        //base.RedState();
    }

    protected override void YellowState()
    {
        player.SetFreeze(false);
        //base.YellowState();
    }
}
