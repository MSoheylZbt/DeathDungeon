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
    [SerializeField] AnimationCurve arrowTrapCounts;
    [SerializeField] AnimationCurve fireTrapCounts;

    [SerializeField] int difficultyLevel = 0;

    [Header("Cache References")]
    public TileBase treasureTile;
    public TileBase openedTreasureTile;
    public TileBase arrowTrapTile;

    public void SetRandomDifficulty()
    {
        float ranomNumber = Random.Range(0.1f, 1f);
        difficultyLevel = (int) difficultyChances.Evaluate(ranomNumber); 
    }

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
}
