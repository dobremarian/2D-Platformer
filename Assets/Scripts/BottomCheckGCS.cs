using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCheckGCS : MonoBehaviour
{
    private PlayerControllerGCS thePlayer;
    //private CameraController theCamera;

    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<PlayerControllerGCS>();
        //theCamera = GameObject.FindObjectOfType<CameraController>();
    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("GroundGCS"))
        {
            thePlayer.IsGrounded = true;

            thePlayer.CanDoubleJump = true;

            //theCamera.IsOnGround = true;
        }

    }
}
