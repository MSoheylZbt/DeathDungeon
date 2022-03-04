using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReact : React
{
    [Header("From ArrowReact")]
    [SerializeField] ArrowTrapManager arrowManager;
    [SerializeField] float yellowStart;
    [SerializeField] float greenStart;
    [SerializeField] float totalLength;

    Coroutine moveCoroutine;

    private void Update()
    {
        if (GetCurrentState() != TimerState.NotStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowReaction();
            }
        }
    }

    protected override void ShowReaction()
    {
        arrowManager.ResetPosition();
        StopCoroutine(moveCoroutine);
        base.ShowReaction();
    }

    public void StartArrowTimer()
    {
        moveCoroutine = StartCoroutine(MoveArrow());
        base.StartTimer(greenStart,yellowStart,totalLength);
    }

    IEnumerator MoveArrow() 
    {
        float arrowSpeed = 0f;
        GameObject arrowObj = new GameObject();
        InitialArrowManager(out arrowSpeed,out arrowObj);
        //print(totalLength);

        while (elapsedTime < totalLength)
        {
            arrowObj.transform.Translate(-transform.up  * arrowSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        player.TakeDamage();
        arrowManager.ResetPosition();
    }

    private void InitialArrowManager(out float arrowAvgSpeed,out GameObject arrowObj)
    {
        arrowManager.SetPlayerTilePos(player.GetPlayerTilePos());
        arrowAvgSpeed = arrowManager.GetAverageVelocity(player.transform.position, totalLength);
        arrowObj = arrowManager.GetCorrectArrow();
    }

}
