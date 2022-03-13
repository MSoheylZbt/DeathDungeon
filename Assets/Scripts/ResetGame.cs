using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    [SerializeField] Knight_Data data;
    LevelManager levelManager;

    public void Init(LevelManager manager)
    {
        levelManager = manager;
    }


    public void RestartGame()
    {

        data.ResetData();
        levelManager.LoadLevel();
        levelManager.ResetLevelIndex();
    }
}
