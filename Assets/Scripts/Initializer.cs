using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        //print("Initializer Start Called");
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.Init();
            GridHandler gridHandler = FindObjectOfType<GridHandler>();
            gridHandler.Init(levelManager);
            ReactManager reactManager = FindObjectOfType<ReactManager>();
            ReflexUI reflexUI = FindObjectOfType<ReflexUI>();
            reactManager.InitReacts(reflexUI);

            ResetGame reseter = FindObjectOfType<ResetGame>();
            reseter.Init(levelManager);
            Knight.instance.Init(gridHandler,reactManager,reseter);

        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            Knight.instance.Init();
        }
    }
}
