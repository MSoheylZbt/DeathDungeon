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
        if(SceneManager.GetActiveScene().buildIndex == 2) // If we are in shop scene
        {
            upgradeTXT.text = buyData.GetUpgradePrice(knightData.UpgradeLevel).ToString();
            healthTXT.text = buyData.healthPotionPrice.ToString();
            InvisTXT.text = buyData.invisiblePotionPrice.ToString();
            greenTXT.text = buyData.greenTimePotionPrice.ToString();
        }
    }

//All functions below, will be called from clicking on shop buttons.

    public void UpgradeArmor()
    {
        if (knightData.Coins < buyData.GetUpgradePrice(knightData.UpgradeLevel)) // if player doesn't have enough money
            return;

        knightData.Coins -= buyData.GetUpgradePrice(knightData.UpgradeLevel);
        knightData.maxHealth++;
        knightData.UpgradeLevel++;
        upgradeTXT.text = buyData.GetUpgradePrice(knightData.UpgradeLevel).ToString(); // Update Shop UI text with new price
    }

    public void BuyHealthPotion()
    {
        if(knightData.Coins < buyData.healthPotionPrice)
            return;

        knightData.Coins -= buyData.healthPotionPrice;
        if (knightData.Health == knightData.maxHealth)
            knightData.HealthPotionCount++;
        else
            knightData.Health++;
    }

    public void BuyInvisiblePotion()
    {
        if (knightData.Coins < buyData.invisiblePotionPrice)
            return;
        knightData.Coins -= buyData.invisiblePotionPrice;
        knightData.InvisPotionCount++;

    }

    public void BuyGreenTimePotionCount()
    {
        if (knightData.Coins < buyData.greenTimePotionPrice)
            return;
        knightData.Coins -= buyData.greenTimePotionPrice;
        knightData.GreePotionCount++;
    }
}
