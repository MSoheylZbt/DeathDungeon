using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetGame : MonoBehaviour
{
    [SerializeField] Knight_Data data;
    LevelManager levelManager;
    RectTransform rectTransform;

    public void Init(LevelManager manager)
    {
        levelManager = manager;
        rectTransform = GetComponent<RectTransform>();
    }


    public void RestartGame()
    {
        data.ResetData();
        levelManager.LoadLevel();
        levelManager.ResetLevelIndex();
        rectTransform.anchoredPosition = new Vector2(-1263f, 0);
    }


    public void ShowMenu()
    {
        rectTransform.anchoredPosition = new Vector2(0,0);
    }
}
