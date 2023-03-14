using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float destroyTime = 2f;

    AudioManager theAudioManager;
    void Start()
    {
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        Destroy(gameObject, destroyTime);
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("EnemyHitbox") || other.CompareTag("Ground") || other.CompareTag("Map") || other.CompareTag("Box"))
        {
            if(other.CompareTag("EnemyHitbox") || other.CompareTag("Box"))
            {
                theAudioManager.PlaySFX(10);
            }
            Destroy(gameObject);
            
        }
    }
}
