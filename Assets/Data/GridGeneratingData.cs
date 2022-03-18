using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Grid Data")]
public class GridGeneratingData : ScriptableObject
{
    [Header("Difficulty realted")]
    //Animation curve is a 2d graph that it's XY values can be changed in unity editor.

    // X (Time) : Chance of selection , Y (Value) : Difficulty
    [SerializeField] AnimationCurve difficultyChances;

    // X (Time) : Difficulty level , Y (Value) : Counts
    [SerializeField] AnimationCurve treasureCounts;
    [SerializeField] AnimationCurve arrowTrapCounts;
    [SerializeField] AnimationCurve fireTrapCounts;
    [SerializeField] AnimationCurve treasureCoins;

    int difficultyLevel = 0;

    [Header("Cache References")]
    //Image references
    public TileBase treasureTile;
    public TileBase openedTreasureTile;
    public TileBase arrowTrapTile;

    /// <summary>
    /// Get a difficulty based on their defined chance in editor.
    /// </summary>
    public void SetRandomDifficulty()
    {
        float ranomNumber = Random.Range(0.1f, 1f);
        difficultyLevel = (int) difficultyChances.Evaluate(ranomNumber); 
    }

//Functions below get correct counts based on selected difficulty
    public int GetTreasureCount()
    {
        return (int) treasureCounts.Evaluate(difficultyLevel);
    }

    public int GetArrowTrapCounts()
    {
        return (int)arrowTrapCounts.Evaluate(difficultyLevel);
    }

    public int GetFireTrapCounts()
    {
        return (int)fireTrapCounts.Evaluate(difficultyLevel);
    }

    public int GetTreasureCoins()
    {
        return (int)treasureCoins.Evaluate(difficultyLevel);
    }
}
