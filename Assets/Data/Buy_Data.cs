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

    public int GetUpgradePrice(int upgradeCount) // With every upgrading, upgrade price will be incremented.
    {
        int finalPrice = firstUpgradePrice + (upgradeCount * upgradeAddition);
        return finalPrice;
    }
}
