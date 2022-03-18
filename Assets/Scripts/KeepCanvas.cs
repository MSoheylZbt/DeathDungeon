using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCanvas : MonoBehaviour
{
    //Canvas is a tools in unity for organizing UI.

    //Keep same instance of Canvas throughout the whole game using singelton pattern.
    static KeepCanvas instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
