using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    private void Start()
    {
        print("Called");
        if(SceneManager.GetActiveScene().buildIndex == 0)
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
            reseter.gameObject.SetActive(false);

        }
        else if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Knight.instance.Init();
        }
    }
}
