using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBGMusic : MonoBehaviour
{
    private void Awake()
    {
        //print("KeepBG Awake");
        //all scripts in unity will be attached to gameObjects as a component, and this function will keep their attached gameObject throughout all scenes.
        DontDestroyOnLoad(gameObject);
    }
}
