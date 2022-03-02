using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Grid Data")]
public class SO_Grid : ScriptableObject
{
    [Header("Difficulty realted")]
    // X (Time) : Chance of selection , Y (Value) : Difficulty
    [SerializeField] AnimationCurve difficultyChances;

    // X (Time) : Difficulty level , Y (Value) : Counts
    [SerializeField] AnimationCurve treasureCounts;
    [SerializeField] AnimationCurve trapCounts;

    [SerializeField] int difficultyLevel = 0;

    [Header("Cache References")]
    public TileBase treasureTile;
    public TileBase openedTreasureTile;
    public TileBase trapTile;

    public void SetRandomDifficulty()
    {
        float ranomNumber = Random.Range(0.1f, 1f);
        difficultyLevel = (int) difficultyChances.Evaluate(ranomNumber); 
    }

    public int GetTreasureCount()
    {
        return (int) treasureCounts.Evaluate(difficultyLevel);
    }

    public int GetTrapCount()
    {
        return (int)trapCounts.Evaluate(difficultyLevel);
    }

}
