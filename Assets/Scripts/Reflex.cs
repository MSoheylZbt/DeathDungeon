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
    [SerializeField] float greenStart;
    [SerializeField] float yellowStart;
    [SerializeField] float totalLength;

    public TimerState currentState = TimerState.NotStarted;

    #region Cache
    public float elapsedTime = 0f;
    bool isTimerStarted = false;
    Coroutine timerCoroutine;
    Knight player;
    #endregion

    public void Init(Knight knight)
    {
        player = knight;
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

    IEnumerator ReflexTimer()
    {
        isTimerStarted = true;

        currentState = TimerState.Red;

        while (elapsedTime < totalLength)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= yellowStart)
                currentState = TimerState.Yellow;
            else if (elapsedTime >= greenStart)
                currentState = TimerState.Green;

            yield return new WaitForEndOfFrame();
        }

        player.TakeDamage();
        ResetCoroutine();
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
                ResetCoroutine();
                break;

            case TimerState.Green:
                StopCoroutine(timerCoroutine);
                print("<color=green> Green Pressed! </color>");
                ResetCoroutine();
                break;

            case TimerState.Yellow:
                StopCoroutine(timerCoroutine);
                print("<color=yellow> Yellow Pressed! </color>");
                ResetCoroutine();
                break;
        }
    }

    void ResetCoroutine()
    {
        elapsedTime = 0;
        currentState = TimerState.NotStarted;
        isTimerStarted = false;
    }
}
