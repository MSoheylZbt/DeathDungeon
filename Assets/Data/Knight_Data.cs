using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Knight_Data : ScriptableObject
{
    public int maxHealth;
    public float greenTimeReduction;
    [SerializeField] int maxStrike;
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


    int currentStrike = 1;
    public int Strike
    {
        get
        {
            return currentStrike;
        }
        set
        {
            if(value <= maxStrike)
                currentStrike = value;
        }
    }

    int currentScore;
    public static event ChangePlayerParams OnGetScore;

    public int Score
    {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore +=  (value * currentStrike);
            OnGetScore?.Invoke();
        }
    }



#endregion

    #region Inventory
    public delegate void Buying(bool isUsed);

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
            int tempCount = invisiblePotionCount;
            invisiblePotionCount = value;
            if (invisiblePotionCount < tempCount && invisiblePotionCount != 0)
                OnSetInvisPotion?.Invoke(true);
            else
                OnSetInvisPotion?.Invoke(false);

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
            int tempCount = healthPotionCount;
            healthPotionCount = value;
            if (healthPotionCount < tempCount && healthPotionCount != 0)
                OnSetHealthPotion?.Invoke(true);
            else
                OnSetHealthPotion?.Invoke(false);
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
            int tempCount = greenTimePotionCount;
            greenTimePotionCount = value;
            if (greenTimePotionCount < tempCount && greenTimePotionCount != 0)
                OnSetGreenPotion?.Invoke(true);
            else
                OnSetGreenPotion?.Invoke(false);
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
            int tempCount = upgradeCount;
            upgradeCount = value;
            if (upgradeCount < tempCount)
                OnSetUpgrade?.Invoke(true);
            else
                OnSetUpgrade?.Invoke(false);
        }
    }
    #endregion

    public void ResetData()
    {
        maxHealth = 1;
        Health = maxHealth;
        Coins = 0;
        InvisPotionCount = 0;
        HealthPotionCount = 0;
        GreePotionCount = 0;
        UpgradeLevel = 0;
        Strike = 1;

        currentScore = 0;
        Score = 0;
    }

}
