using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]
public class Knight_Data : ScriptableObject
{
    [Header("Playtime parameters")]
    public int currentCoins;
    public int currentHealth;
    public int invisiblePotionCount;
    public int healthPotionCount;
    public int greenTimePotionCount;
    public float greenTimeReduction;

    [Header("Player Parameters")]
    public int maxHealth = 4;
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
