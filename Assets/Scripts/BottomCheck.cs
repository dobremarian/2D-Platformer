using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheck : MonoBehaviour
{
    private PlayerController thePlayer;
    //private CameraController theCamera;

    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<PlayerController>();
        //theCamera = GameObject.FindObjectOfType<CameraController>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Box") || other.gameObject.CompareTag("Start"))
        {
                thePlayer.IsGrounded = true;

                thePlayer.CanDoubleJump = true;

                //theCamera.IsOnGround = true;
            }
        
    }
}
