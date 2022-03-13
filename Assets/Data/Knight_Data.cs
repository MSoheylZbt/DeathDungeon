using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Knight_Data : ScriptableObject
{
    public int maxHealth;
    public float greenTimeReduction;
    [HideInInspector] public Vector2 moveAmount;
    [HideInInspector] public Vector3 playerFirstPos;

    #region Player Parameters
    public delegate void ChangePlayerParams();

    int currentCoins;
    public static event ChangePlayerParams OnCoinUsed;
    public int Coins
    {
        get
        {
            return currentCoins;
        }
        set
        {
            currentCoins = value;
            OnCoinUsed?.Invoke();
        }
    }

    int currentHealth;
    public static event ChangePlayerParams OnHeartUsed;
    public int Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
            OnHeartUsed?.Invoke();
        }
    }

#endregion

    #region Inventory
    public delegate void Buying();

    int invisiblePotionCount;
    public static event Buying OnSetInvisPotion;
    public int InvisPotionCount
    {
        get
        {
            return invisiblePotionCount;
        }
        set
        {
            invisiblePotionCount = value;
            OnSetInvisPotion?.Invoke();
        }
    }

    int healthPotionCount;
    public static event Buying OnSetHealthPotion;

    public int HealthPotionCount
    {
        get
        {
            return healthPotionCount;
        }
        set
        {
            healthPotionCount = value;
            OnSetHealthPotion?.Invoke();
        }
    }

    int greenTimePotionCount;
    public static event Buying OnSetGreenPotion;

    public int GreePotionCount
    {
        get
        {
            return greenTimePotionCount;
        }
        set
        {
            greenTimePotionCount = value;
            OnSetGreenPotion?.Invoke();
        }
    }

    int upgradeCount;
    public static event Buying OnSetUpgrade;

    public int UpgradeLevel
    {
        get
        {
            return upgradeCount;
        }
        set
        {
            upgradeCount = value;
            OnSetUpgrade?.Invoke();
        }
    }
    #endregion

    public void ResetData()
    {
        maxHealth = 2;
        Health = maxHealth;
        Coins = 0;
        InvisPotionCount = 0;
        HealthPotionCount = 0;
        GreePotionCount = 0;
        UpgradeLevel = 0;
    }

}
