using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndObject : MonoBehaviour
{
    Animator endAnim;
    GameManager theGM;
    
    void Start()
    {
        endAnim = GetComponent<Animator>();
        theGM = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endAnim.SetTrigger("EndMove_T");
            theGM.LevelComplete();
        }
    }
}
