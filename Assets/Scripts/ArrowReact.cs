using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReact : React //Inherited from React Class
{
    [Header("From ArrowReact")]
    ArrowTrapManager arrowManager;
    [SerializeField] float yellowStart; //Time of Yellow state starting in second
    [SerializeField] float greenStart;
    [SerializeField] float totalLength;//total Length of Timer

    Coroutine moveCoroutine;

    private void Update() // Check for player input and current state every frame.
    {
        if (GetCurrentState() != TimerState.NotStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Check for Space pressing Key
            {
                ShowReaction(); //Call parent class function
            }
        }
    }

    public void Init(ArrowTrapManager arrowTrapManager,ReflexUI refUi)
    {
        base.Init(refUi);
        arrowManager = arrowTrapManager;
    }

    protected override void ShowReaction()
    {
        arrowManager.ResetPosition();
        StopCoroutine(moveCoroutine);
        base.ShowReaction();
    }

    /// <summary>
    /// greenTimeReduction is used when a greenTime potion used otherwise it should be 0.
    /// </summary>
    /// <param name="greenTimeReduction"></param>
    public void StartArrowTimer(float greenTimeReduction)
    {
        float tempGreenStart = greenStart - greenTimeReduction;
        moveCoroutine = StartCoroutine(MoveArrow());
        base.StartTimer(tempGreenStart,yellowStart,totalLength);
    }

    /// <summary>
    /// This Coroutin Move Arrow with relative speed according to Timer total length.
    /// this corouting started at same time with Timer coroutine.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveArrow() 
    {
        arrowManager.SetPlayerTilePosRef(player.GetPlayerTilePos());
        float arrowSpeed = arrowManager.GetAverageVelocity(player.transform.position, totalLength);
        GameObject arrowObj = arrowManager.GetCorrectArrow();

        while (elapsedTime < totalLength)
        {
            arrowObj.transform.Translate(-transform.up  * arrowSpeed * Time.deltaTime); // Move arrow in down direction and with speed of arrow speed.
                                                                                        // Multiplied by Time.deltaTime because of framerate indepentcy,
            yield return new WaitForEndOfFrame();
        }

        //When Timer end and player doesn't show any reaction.
        player.TakeDamage();
        player.SetFreeze(false);
        arrowManager.ResetPosition();
    }
}
