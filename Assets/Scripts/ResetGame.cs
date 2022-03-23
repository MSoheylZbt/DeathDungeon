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

    public void RestartGame() // Called from UI Button
    {
        data.ResetData();
        levelManager.ResetLevelIndex();
        levelManager.LoadFirstLevel();
        rectTransform.anchoredPosition = new Vector2(-1718f, 0); // Move menu out of Screen.
    }

    public void ShowMenu()
    {
        rectTransform.anchoredPosition = new Vector2(0,0); // move menu in middle of screen.
        ShowScore();
    }

    void ShowScore()
    {
        scoreText.text = data.Score.ToString();
    }
}
