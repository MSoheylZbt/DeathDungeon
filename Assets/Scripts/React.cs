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


public class React : MonoBehaviour
{
    [Header("From React")]
    [SerializeField] ReflexUI reflexUI;

    TimerState currentState = TimerState.NotStarted;

    #region Cache
    protected float elapsedTime = 0f;
    protected Knight player;
    bool isTimerStarted = false;
    Coroutine timerCoroutine;

    float tempGreenStart = 0f;
    float tempYellowStart = 0f;
    float tempTotalLength = 0f;

    #endregion

    public delegate void TimerEnd();
    public static event TimerEnd OnTimerEnd;

    public void Init(Knight knight)
    {
        player = knight;
        reflexUI.Init();
    }

    protected void StartTimer(float greenStartToSet,float yellowStartToSet,float length)
    {
        if (isTimerStarted == false)
        {
            tempGreenStart = greenStartToSet;
            tempYellowStart = yellowStartToSet;
            tempTotalLength = length;
            reflexUI.SetSliderMaxValue(tempTotalLength);

            timerCoroutine = StartCoroutine(Timer());
        }
    }


    IEnumerator Timer() //Side-effect: Moving Arrow
    {
        isTimerStarted = true;

        SetCurrentState(TimerState.Red, Color.red);

        while (elapsedTime < tempTotalLength)
        {
            //print("Timer Started " + elapsedTime);
            elapsedTime += Time.deltaTime;
            reflexUI.SetSliderFiller(elapsedTime);

            if (elapsedTime >= tempGreenStart)
                SetCurrentState(TimerState.Green, Color.green);
            else if (elapsedTime >= tempYellowStart)
                SetCurrentState(TimerState.Yellow, Color.yellow);

            yield return new WaitForEndOfFrame();
        }
        ResetTimer();

        if(OnTimerEnd != null)
            OnTimerEnd();
    }

    protected virtual void ShowReaction()
    {
        switch (currentState)
        {
            case TimerState.NotStarted:
                break;

            case TimerState.Red:
                StopCoroutine(timerCoroutine);
                ResetTimer();
                break;

            case TimerState.Green:
                StopCoroutine(timerCoroutine);
                print("<color=green> Green Pressed! </color>");
                ResetTimer();
                break;

            case TimerState.Yellow:
                StopCoroutine(timerCoroutine);
                print("<color=yellow> Yellow Pressed! </color>");
                ResetTimer();
                break;
        }
    }


    void ResetTimer()
    {
        SetCurrentState(TimerState.NotStarted);
        ResetCoroutine();
        ResetUI();
    }

    void ResetCoroutine()
    {
        elapsedTime = 0;
        isTimerStarted = false;
    }

    void ResetUI()
    {
        reflexUI.SetSliderColor(Color.white);
        reflexUI.SetSliderFiller(0);
        reflexUI.SetSliderText("");
    }


    private void SetCurrentState(TimerState statetoSet, Color stateColor)
    {
        currentState = statetoSet;
        reflexUI.SetSliderColor(stateColor);
        SetStateText();
    }

    public void SetStateText()
    {
        if (currentState == TimerState.Red)
            reflexUI.SetSliderText("Wait");
        else
            reflexUI.SetSliderText("Hit");
    }

    protected void SetCurrentState(TimerState statetoSet)
    {
        currentState = statetoSet;
    }

    public TimerState GetCurrentState()
    {
        return currentState;
    }

}
