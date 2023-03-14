using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RockHead : Enemy
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToPlayer);
        }

    }
}
