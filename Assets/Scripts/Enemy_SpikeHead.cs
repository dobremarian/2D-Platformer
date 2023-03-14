using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpikeHead : Enemy
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToPlayer);
        }
    }
}
