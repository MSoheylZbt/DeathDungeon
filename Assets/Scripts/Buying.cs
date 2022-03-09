using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        upgradeTXT.text = buyData.GetUpgradePrice().ToString();
        healthTXT.text = buyData.healthPotionPrice.ToString();
        InvisTXT.text = buyData.invisiblePotionPrice.ToString();
        greenTXT.text = buyData.greenTimePotionPrice.ToString();
    }

    public void UpgradeArmor()
    {
        if (knightData.currentCoins < buyData.GetUpgradePrice())
            return;

        knightData.currentCoins -= buyData.GetUpgradePrice();
        knightData.maxHealth++;

        buyData.IncrementUpgrade();

        upgradeTXT.text = buyData.GetUpgradePrice().ToString();
    }

    public void BuyHealthPotion()
    {
        if(knightData.currentCoins < buyData.healthPotionPrice)
            return;
        knightData.currentCoins -= buyData.healthPotionPrice;
        if (knightData.currentHealth == knightData.maxHealth)
            knightData.healthPotionCount++;
        else
            knightData.currentHealth++;
    }

    public void BuyInvisiblePotion()
    {
        if (knightData.currentCoins < buyData.invisiblePotionPrice)
            return;
        knightData.currentCoins -= buyData.invisiblePotionPrice;
        knightData.invisiblePotionCount++;
    }

    public void BuyGreenTimePotionCount()
    {
        if (knightData.currentCoins < buyData.greenTimePotionPrice)
            return;
        knightData.currentCoins -= buyData.greenTimePotionPrice;
        knightData.greenTimePotionCount++;
    }
}
