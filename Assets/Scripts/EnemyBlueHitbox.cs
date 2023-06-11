using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueHitbox : MonoBehaviour
{
    [SerializeField] EnemyBlue enemyB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            enemyB.EnemyBlueTriggerHit();
            Destroy(other.gameObject);
        }
    }
}
