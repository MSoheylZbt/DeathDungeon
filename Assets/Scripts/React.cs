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

//React Class will be Used as a parent class.
public class React : MonoBehaviour
{
    [Header("From React")]
    [SerializeField] int greenCoins; // Amount of coins that player gets when he/she react in green time.
    [SerializeField] int greenScore;// Amount of Score that player gets when he/she react in green time.
    [SerializeField] int yellowScore;
    public ReflexUI reflexUI;

    TimerState currentState = TimerState.NotStarted;

    #region Cache
    protected float elapsedTime = 0f; // elapsed time from when a timer start.
    protected Knight player;
    bool isTimerStarted = false; // for not starting multiple coroutines at same time.
    Coroutine timerCoroutine;

    float tempGreenStart = 0f;
    float tempYellowStart = 0f;
    float tempTotalLength = 0f;

    #endregion


    public void Init(ReflexUI refUI) //Initialize all needed references
    {
        player = Knight.instance;
        reflexUI = refUI;
        reflexUI.Init();
    }

    /// <summary>
    /// Start a timer with values from input.
    /// </summary>
    /// <param name="greenStartToSet"></param>
    /// <param name="yellowStartToSet"></param>
    /// <param name="length"></param>
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

    /// <summary>
    /// A coroutine that act as a timer and set Timer states.
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        isTimerStarted = true;
        SetCurrentState(TimerState.Red, Color.red);
        RedState();

        while (elapsedTime < tempTotalLength)
        {
            elapsedTime += Time.deltaTime;//Time.deltaTime is time difference between two frame.
            reflexUI.SetSliderFiller(elapsedTime);

            //These if else checks for reaching GreenState or YellowState. 
            if (elapsedTime >= tempGreenStart)
                SetCurrentState(TimerState.Green, Color.green);
            else if (elapsedTime >= tempYellowStart)
            {
                SetCurrentState(TimerState.Yellow, Color.yellow);
                YellowState();
            }

            yield return new WaitForEndOfFrame();//Pause coroutine for one frame.
        }
        ResetTimer();

        TimerEnd();
    }

    /// <summary>
    /// Show specific designed reaction according to current timer state.
    /// All calling functions in states are based on game design.
    /// </summary>
    protected virtual void ShowReaction()
    {
        switch (currentState)
        {
            case TimerState.NotStarted:
                break;

            case TimerState.Red:
                player.TakeDamage();
                player.SetFreeze(false);
                player.ResetStrike();
                ResetTimer();
                break;

            case TimerState.Green:
                ResetTimer();
                player.SetFreeze(false);
                player.PlayGreenShieldAnimation();
                player.AddCoins(greenCoins);
                player.SetScore(greenScore);
                player.IncrementStrike(reflexUI);
                //print("<color=green> Green Pressed! </color>");
                break;

            case TimerState.Yellow:
                ResetTimer();
                player.SetFreeze(false);
                player.PlayYellowShieldAnimation();
                player.ResetStrike();
                player.SetScore(yellowScore);
                //print("<color=yellow> Yellow Pressed! </color>");
                break;
        }
    }

    /// <summary>
    /// Resetes all Timer related parameters.
    /// </summary>
    void ResetTimer()
    {
        StopCoroutine(timerCoroutine);
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

    /// <summary>
    /// Set Current state of timer and it's specified color
    /// </summary>
    /// <param name="statetoSet"></param>
    /// <param name="stateColor"></param>
    private void SetCurrentState(TimerState statetoSet, Color stateColor)
    {
        currentState = statetoSet;
        reflexUI.SetSliderColor(stateColor);
    }
    protected void SetCurrentState(TimerState statetoSet)
    {
        currentState = statetoSet;
    }

    public TimerState GetCurrentState()
    {
        return currentState;
    }

//Functions below are written just for overrding in childs classes. Can use Events instead.
    protected virtual void TimerEnd()
    {

    }

    protected virtual void RedState()
    {

    }

    protected virtual void YellowState()
    {

    }
}
