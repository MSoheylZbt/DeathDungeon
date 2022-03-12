using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReact : React
{
    [Header("From ArrowReact")]
    ArrowTrapManager arrowManager;
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

    public void StartArrowTimer(float greenTimeReduction)
    {
        float tempGreenStart = greenStart - greenTimeReduction;
        moveCoroutine = StartCoroutine(MoveArrow());
        base.StartTimer(tempGreenStart,yellowStart,totalLength);
    }

    IEnumerator MoveArrow() 
    {
        arrowManager.SetPlayerTilePos(player.GetPlayerTilePos());
        float arrowSpeed = arrowManager.GetAverageVelocity(player.transform.position, totalLength);
        GameObject arrowObj = arrowManager.GetCorrectArrow();
        //print(totalLength);

        while (elapsedTime < totalLength)
        {
            arrowObj.transform.Translate(-transform.up  * arrowSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        player.TakeDamage();
        player.SetFreeze(false);
        arrowManager.ResetPosition();
    }


}
