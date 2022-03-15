using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBGMusic : MonoBehaviour
{
    private void Awake()
    {
        //print("KeepBG Awake");
        DontDestroyOnLoad(gameObject);
    }
}
