using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireReact : React
{
    [Header("From FireReact")]
    [SerializeField] TileBase fireTile; // Image of fireTile
    [SerializeField] float yellowStart;
    [SerializeField] float greenStart;
    [SerializeField] float totalLength;

    Vector3 playerCurrentTilePos = new Vector3();

    private void OnEnable() // Called when this object get active in scene.
    {
        Knight.OnMove += ShowReaction; //Show reaction function is a listener of OnMove event and called when OnMove invoked.
    }

    protected override void ShowReaction()
    {
        if(GetCurrentState() != TimerState.NotStarted)
        {
            base.ShowReaction();
            SetFireTileImage();
        }
    }

    public void StartFireTimer(float greenTimeReduction)
    {
        playerCurrentTilePos = player.transform.position;
        float tempGreenStart = greenStart - greenTimeReduction;
        base.StartTimer(tempGreenStart,yellowStart,totalLength);
    }


    /// <summary>
    /// Set Fire image on player current tile position.
    /// </summary>
    private void SetFireTileImage()
    {
        Tilemap tilemap = player.GetTileMap();
        tilemap.SetTile(tilemap.WorldToCell(playerCurrentTilePos), fireTile);
    }

    /// <summary>
    /// Set Fire image on given position.
    /// </summary>
    public void SetTileFireImage(Vector3 playerPos)
    {
        Tilemap tilemap = player.GetTileMap();
        tilemap.SetTile(tilemap.WorldToCell(playerPos), fireTile);
    }

    protected override void TimerEnd()
    {
        KillWithFireTrap();
    }

    private void KillWithFireTrap()
    {
        SetFireTileImage();
        player.Die();
    }


    protected override void RedState()
    {
        player.SetFreeze(true);
    }

    protected override void YellowState()
    {
        player.SetFreeze(false);
    }

    private void OnDisable() // Called when scene changed or object get disabled
    {
        Knight.OnMove -= ShowReaction;
    }
}
