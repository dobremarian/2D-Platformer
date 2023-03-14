using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float bounceForce;

    Rigidbody2D playerRb;
    Animator trampolineAnim;


    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        trampolineAnim = GetComponent<Animator>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            trampolineAnim.SetTrigger("Moving_T");
        }
    }
}
