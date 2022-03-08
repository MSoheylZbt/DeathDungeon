using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Knight_Data : ScriptableObject
{
    public int maxHealth = 4;
    public int currentHealth;

    public int currentCoins;
    public int invisiblePotionCount;
    public int healthPotionCount;
    public int greenTimePotionCount;
    public float greenTimeReduction;
    public Vector2 moveAmount;

    public void ResetData()
    {
        currentHealth = maxHealth;
        currentCoins = 0;
        invisiblePotionCount = 0;
        healthPotionCount = 0;
        greenTimePotionCount = 0;
    }
}
