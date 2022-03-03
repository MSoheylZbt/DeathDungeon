using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerState
{
    NotStarted,
    Red,
    Green,
    Yellow
}

public class Reflex : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] float yellowStart;
    [SerializeField] float greenStart;
    [SerializeField] float totalLength;
    [SerializeField] ReflexUI reflexUI;
    [SerializeField] ArrowTrapManager arrowManager;

    TimerState currentState = TimerState.NotStarted;

    #region Cache
    float elapsedTime = 0f;
    bool isTimerStarted = false;
    Coroutine timerCoroutine;
    Knight player;
    #endregion

    public void Init(Knight knight)
    {
        player = knight;
        reflexUI.Init(totalLength);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ReflexToTraps();
        }
    }

    public void StartTimer()
    {
        if(isTimerStarted == false)
            timerCoroutine = StartCoroutine(ReflexTimer());
    }

    IEnumerator ReflexTimer() //Side-effect: Moving Arrow
    {
        isTimerStarted = true;

        SetCurrentState(TimerState.Red, Color.red);

        float arrowSpeed = 0f;
        GameObject arrowObj = new GameObject();
        InitialArrowManager(out arrowSpeed,out arrowObj);

        while (elapsedTime < totalLength)
        {
            elapsedTime += Time.deltaTime;
            reflexUI.SetSliderFiller(elapsedTime);

            arrowObj.transform.Translate(-transform.up  * arrowSpeed * Time.deltaTime);

            if (elapsedTime >= greenStart)
                SetCurrentState(TimerState.Green, Color.green);
            else if (elapsedTime >= yellowStart)
                SetCurrentState(TimerState.Yellow, Color.yellow);

            yield return new WaitForEndOfFrame();
        }

        player.TakeDamage();
        arrowManager.ResetPosition();
        ResetTimer();
    }

    void ReflexToTraps()
    {
        switch (currentState)
        {
            case TimerState.NotStarted:
                break;

            case TimerState.Red:
                StopCoroutine(timerCoroutine);
                player.TakeDamage();
                arrowManager.ResetPosition();
                ResetTimer();
                break;

            case TimerState.Green:
                StopCoroutine(timerCoroutine);
                print("<color=green> Green Pressed! </color>");
                arrowManager.ResetPosition();
                ResetTimer();
                break;

            case TimerState.Yellow:
                StopCoroutine(timerCoroutine);
                print("<color=yellow> Yellow Pressed! </color>");
                arrowManager.ResetPosition();
                ResetTimer();
                break;
        }
    }

    void ResetTimer()
    {
        ResetCoroutine();
        ResetUI();
    }

    void ResetCoroutine()
    {
        elapsedTime = 0;
        currentState = TimerState.NotStarted; // SetCurrentState not used for avoiding switch and readibility of ResetUI
        isTimerStarted = false;
    }

    void ResetUI()
    {
        reflexUI.SetSliderColor(Color.white);
        reflexUI.SetSliderFiller(0);
        reflexUI.SetSliderText("");
    }

    private void SetCurrentState(TimerState statetoSet,Color stateColor)
    {
        currentState = statetoSet;
        reflexUI.SetSliderColor(stateColor);

        if (currentState == TimerState.Red)
            reflexUI.SetSliderText("Wait");
        else
            reflexUI.SetSliderText("Hit");

    }

    private void InitialArrowManager(out float arrowAvgSpeed,out GameObject arrowObj)
    {
        arrowManager.SetPlayerTilePos(player.GetPlayerTilePos());
        arrowAvgSpeed = arrowManager.GetAverageVelocity(player.transform.position, totalLength);
        arrowObj = arrowManager.GetCorrectArrow();
    }

    public TimerState GetCurrentState()
    {
        return currentState;
    }
}
