using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Buying : MonoBehaviour
{
    [SerializeField] Knight_Data knightData;
    [SerializeField] Buy_Data buyData;

    [Header("Price Texts")]
    [SerializeField] TextMeshProUGUI upgradeTXT;
    [SerializeField] TextMeshProUGUI healthTXT;
    [SerializeField] TextMeshProUGUI InvisTXT;
    [SerializeField] TextMeshProUGUI greenTXT;


    private void Awake()
    {

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            upgradeTXT.text = buyData.GetUpgradePrice(knightData.UpgradeLevel).ToString();
            healthTXT.text = buyData.healthPotionPrice.ToString();
            InvisTXT.text = buyData.invisiblePotionPrice.ToString();
            greenTXT.text = buyData.greenTimePotionPrice.ToString();
        }
    }

    public void UpgradeArmor()
    {
        if (knightData.currentCoins < buyData.GetUpgradePrice(knightData.UpgradeLevel))
            return;

        knightData.currentCoins -= buyData.GetUpgradePrice(knightData.UpgradeLevel);
        knightData.maxHealth++;

        knightData.UpgradeLevel++;

        upgradeTXT.text = buyData.GetUpgradePrice(knightData.UpgradeLevel).ToString();
    }

    public void BuyHealthPotion()
    {
        if(knightData.currentCoins < buyData.healthPotionPrice)
            return;

        knightData.currentCoins -= buyData.healthPotionPrice;
        if (knightData.currentHealth == knightData.maxHealth)
            knightData.HealthPotionCount++;
        else
            knightData.currentHealth++;
    }

    public void BuyInvisiblePotion()
    {
        if (knightData.currentCoins < buyData.invisiblePotionPrice)
            return;
        knightData.currentCoins -= buyData.invisiblePotionPrice;
        knightData.InvisPotionCount++;

    }

    public void BuyGreenTimePotionCount()
    {
        if (knightData.currentCoins < buyData.greenTimePotionPrice)
            return;
        knightData.currentCoins -= buyData.greenTimePotionPrice;
        knightData.GreePotionCount++;
    }
}
