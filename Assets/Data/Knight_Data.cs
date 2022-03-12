using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Knight_Data : ScriptableObject
{
    [Header("Playtime parameters")]
    public int currentCoins;
    public int currentHealth;


    [Header("Player Parameters")]
    public int maxHealth = 4;
    public Vector2 moveAmount;
    public Vector3 playerFirstPos;
    public float greenTimeReduction;

    public delegate void Buying();
    public static event Buying OnSetInvisPotion;
    public static event Buying OnSetHealthPotion;
    public static event Buying OnSetUpgrade;
    public static event Buying OnSetGreenPotion;

    [SerializeField] int invisiblePotionCount;
    public int InvisPotionCount
    {
        get
        {
            return invisiblePotionCount;
        }
        set
        {
            invisiblePotionCount = value;
            OnSetInvisPotion();
        }
    }

    [SerializeField] int healthPotionCount;
    public int HealthPotionCount
    {
        get
        {
            return healthPotionCount;
        }
        set
        {
            healthPotionCount = value;
            OnSetHealthPotion();
        }
    }

    [SerializeField] int greenTimePotionCount;
    public int GreePotionCount
    {
        get
        {
            return greenTimePotionCount;
        }
        set
        {
            greenTimePotionCount = value;
            OnSetGreenPotion();
        }
    }

    [SerializeField] int upgradeCount;
    public int UpgradeLevel
    {
        get
        {
            return upgradeCount;
        }
        set
        {
            upgradeCount = value;
            OnSetUpgrade();
        }
    }

    public void ResetData()
    {
        currentHealth = maxHealth;
        currentCoins = 0;
        invisiblePotionCount = 0;
        healthPotionCount = 0;
        greenTimePotionCount = 0;
    }

}
