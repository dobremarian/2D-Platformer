using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaskHitbox : MonoBehaviour
{
    [SerializeField] EnemyMask enemyM;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            enemyM.EnemyMaskTriggerHit();
            Destroy(other.gameObject);
        }
    }
}
