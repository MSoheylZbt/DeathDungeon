using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Sounds")]
public class Sounds_data : ScriptableObject
{
    [Header("Walking")]
    public AudioClip walkSound;
    public float walkAmount;

    [Header("Coin")]
    public AudioClip coinSound;
    public float coinAmount;

    [Header("Damage")]
    public AudioClip damageSound;
    public float damageAmount;

    [Header("Arrow Trap")]
    public AudioClip arrowSound;
    public float arrowAmount;

    [Header("Fire Trap")]
    public AudioClip fireSound;
    public float fireAmount;

    [Header("Potion")]
    public AudioClip potionSound;
    public float potionAmount;

    [Header("Shield")]
    public AudioClip shieldSound;
    public float shieldAmount;
}
