using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    string direction = string.Empty;
    public string Direction { get { return direction; }
        set { direction = value; }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Projectile"))
        {
            enemy.HitboxHit(direction);
            enemy.TakeDamage();
            Debug.Log("Hit");
        }
    }
}
