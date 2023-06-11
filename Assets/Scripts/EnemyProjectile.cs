using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private GameManager theGM;

    private int damageToPlayer;
    float destroyTime = 2f;

    public int DamageToPlayer
    {
        set { damageToPlayer = value; }
    }

    void Start()
    {
        theGM = GameObject.FindObjectOfType<GameManager>();
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Map"))
        {   
            Destroy(gameObject);
        }
        if (other.gameObject.transform.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToPlayer);
            Destroy(gameObject);
        }
    }
}
