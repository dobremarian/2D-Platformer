using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    PlayerController thePlayer;
    CameraController theCamera;
    [SerializeField] Transform stopFollowingPlayerPoint;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(thePlayer.gameObject.GetComponent<Transform>().position, stopFollowingPlayerPoint.position) < 0.2f)
        {
            theCamera.FollowPlayer = false;
        }
        else
        {
            theCamera.FollowPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            thePlayer.ExitFan();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            thePlayer.EnterFan();
        }
    }
}
