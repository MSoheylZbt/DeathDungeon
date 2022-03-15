using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResetGame : MonoBehaviour
{
    [SerializeField] Knight_Data data;
    [SerializeField] TextMeshProUGUI scoreText;
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
        levelManager.ResetLevelIndex();
        levelManager.LoadLevel();
        rectTransform.anchoredPosition = new Vector2(-1263f, 0);
    }

    public void ShowMenu()
    {
        rectTransform.anchoredPosition = new Vector2(0,0);
        ShowScore();
    }

    void ShowScore()
    {
        scoreText.text = data.Score.ToString();
    }
}
