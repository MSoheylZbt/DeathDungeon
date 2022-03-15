using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBGMusic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
