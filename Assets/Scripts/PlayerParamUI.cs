using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerParamUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinTxt;
    [SerializeField] TextMeshProUGUI heartTxt;
    [SerializeField] TextMeshProUGUI scoreTxt;

    [SerializeField] Knight_Data data;

    private void OnEnable()
    {
        Knight_Data.OnCoinUsed += ChangeCoinText;
        Knight_Data.OnHeartUsed += ChangeHeartText;
        Knight_Data.OnGetScore += ChangeScoreText;
    }

    void ChangeHeartText()
    {
        heartTxt.text = data.Health.ToString();
    }

    void ChangeCoinText()
    {
        coinTxt.text = data.Coins.ToString();
    }

    void ChangeScoreText()
    {
        scoreTxt.text = data.Score.ToString();
    }

    private void OnDisable()
    {
        Knight_Data.OnCoinUsed -= ChangeCoinText;
        Knight_Data.OnHeartUsed -= ChangeHeartText;
        Knight_Data.OnGetScore -= ChangeScoreText;
    }
}
