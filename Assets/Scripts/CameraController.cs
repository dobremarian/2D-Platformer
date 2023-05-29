using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class CameraController : MonoBehaviour
{
    [SerializeField] float xLimit = 6.56f;
    [SerializeField] float yLimit = 2.15f;
    private float yOffset = 0.8f;

    private Vector2 velocity = Vector2.zero;
    [SerializeField] float smoothTime;

    private bool followPlayer = true;

    public bool FollowPlayer
    {
        set { followPlayer = value; }
    }


    private Transform playerTR;
    private Transform cameraTR;

    void Start()
    {
        cameraTR = gameObject.GetComponent<Transform>();
        playerTR = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }

    
    void FixedUpdate()
    {
        if(followPlayer)
        {
            float posX = Mathf.SmoothDamp(cameraTR.position.x, playerTR.position.x, ref velocity.x, smoothTime);
            float posY = Mathf.SmoothDamp(cameraTR.position.y, playerTR.position.y + yOffset, ref velocity.y, smoothTime);

            cameraTR.position = new Vector3(posX, posY, cameraTR.position.z);

            if (cameraTR.position.x < -xLimit)
            {
                cameraTR.position = new Vector3(-xLimit, cameraTR.position.y, cameraTR.position.z);
            }
            if (cameraTR.position.x > xLimit)
            {
                cameraTR.position = new Vector3(xLimit, cameraTR.position.y, cameraTR.position.z);
            }
            if (cameraTR.position.y < -yLimit)
            {
                cameraTR.position = new Vector3(cameraTR.position.x, -yLimit, cameraTR.position.z);
            }
            if (cameraTR.position.y > yLimit)
            {
                cameraTR.position = new Vector3(cameraTR.position.x, yLimit, cameraTR.position.z);
            }
        }

    }
}
