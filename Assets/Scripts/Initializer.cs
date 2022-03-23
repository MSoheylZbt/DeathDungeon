using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    //Each time scene loaded, this function will called and set all references in all Objects with calling their init funcitons.
    [SerializeField] Knight knightPref;
    [SerializeField] GridHandler gridPref;

    private void Start()
    {
        if(Knight.instance == null)
        {
            Instantiate(knightPref, new Vector2(1.55f, -4.45f), Quaternion.identity);
        }

        if(SceneManager.GetActiveScene().buildIndex == 1) // if we are in main scene
        {
            //FindObjectOfType loop through all ojects in scene and Find an object with given type and return first finded object.
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.Init();

            GridHandler gridHandler = Instantiate(gridPref,new Vector2(0,0),Quaternion.identity);
            gridHandler.Init(levelManager);
            levelManager.SetGridHandler(gridHandler);

            ReactManager reactManager = FindObjectOfType<ReactManager>();

            ReflexUI reflexUI = FindObjectOfType<ReflexUI>();
            reactManager.InitReacts(reflexUI);

            ResetGame reseter = FindObjectOfType<ResetGame>();
            reseter.Init(levelManager);

            Knight.instance.Init(gridHandler,reactManager,reseter);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2) // if we are in shop scene
        {
            Knight.instance.Init();
        }
    }
}
