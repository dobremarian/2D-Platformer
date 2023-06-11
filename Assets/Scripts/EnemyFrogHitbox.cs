using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrogHitbox : MonoBehaviour
{
    [SerializeField] EnemyFrog enemyF;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            enemyF.EnemyFrogTriggerHit();
            Destroy(other.gameObject);
        }
    }
}
