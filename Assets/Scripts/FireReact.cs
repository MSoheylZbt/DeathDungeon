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

    Vector3 playerCurrentTilePos = new Vector3();

    private void OnEnable()
    {
        Knight.OnMove += ShowReaction;
    }


    protected override void ShowReaction()
    {
        if(GetCurrentState() != TimerState.NotStarted)
        {
            //Debug.Log("Call from " + "<color=black> FireReact: </color>" + "<color=Green> Show Reaction! </color>");
            base.ShowReaction();
            SetTileFireImage();
        }
    }

    public void StartFireTimer(float greenTimeReduction)
    {
        playerCurrentTilePos = player.transform.position;
        //Debug.Log("Call from " + "<color=black> FireReact: </color>" + "<color=green> Current position: </color> " + playerCurrentTilePos);
        float tempGreenStart = greenStart - greenTimeReduction;
        base.StartTimer(tempGreenStart,yellowStart,totalLength);
    }

    private void KillWithFireTrap()
    {
        SetTileFireImage();
        player.Die();
    }

    private void SetTileFireImage()
    {
        //Debug.Log("Call from " + "<color=black> FireReact: </color>" + "<color=red> Tile Set! </color>");
        Tilemap tilemap = player.GetTileMap();
        tilemap.SetTile(tilemap.WorldToCell(playerCurrentTilePos), fireTile);
    }

    public void SetTileFireImage(Vector3 playerPos)
    {
        Tilemap tilemap = player.GetTileMap();
        tilemap.SetTile(tilemap.WorldToCell(playerPos), fireTile);
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

    private void OnDisable()
    {
        Knight.OnMove -= ShowReaction;
    }
}
