using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ArrowDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Knight player =  collision.gameObject.GetComponent<Knight>();
        if(player)
            player.TakeDamage();
    }
}
