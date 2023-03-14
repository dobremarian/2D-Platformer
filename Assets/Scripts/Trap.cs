using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected GameManager theGM;
    [SerializeField] protected int damageToDeal;


    virtual protected void Start()
    {
        theGM = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.transform.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToDeal);
        }
    }
}
