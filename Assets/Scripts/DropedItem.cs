using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedItem : MonoBehaviour
{

    [SerializeField] int healthAmount;
    [SerializeField] int scoreAmount;
    [SerializeField] GameObject desappearEffect;
    GameManager theGM;
    AudioManager theAudioManager;

    void Start()
    {
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            theAudioManager.PlaySFX(3);
            Destroy(gameObject);
            Instantiate(desappearEffect, gameObject.transform.position, desappearEffect.transform.rotation);
            theGM.PlayerHP += healthAmount;
            theGM.PlayerScore += scoreAmount;
            theGM.UpdateStats();
        }
    }
}
