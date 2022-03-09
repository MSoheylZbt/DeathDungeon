using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prices")]
public class Buy_Data : ScriptableObject
{
    public int healthPotionPrice;
    public int invisiblePotionPrice;
    public int greenTimePotionPrice;

    [SerializeField] int firstUpgradePrice;
    [SerializeField] int upgradeAddition;
    [SerializeField] int upgradeCount = 0;

    public int GetUpgradePrice()
    {
        int finalPrice = firstUpgradePrice + (upgradeCount * upgradeAddition);
        return finalPrice;
    }

    public void IncrementUpgrade()
    {
        upgradeCount++;
    }
}
