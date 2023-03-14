using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform pointOne, pointTwo;
    bool isAtPointOne = false;
    Vector3 target;
    [SerializeField] float speed;
    [SerializeField] float waitTime;
    float waitCountdown;

    Animator platformAnim;

    void Start()
    {
        target = pointOne.position;
        pointOne.parent = null;
        pointTwo.parent = null;

        platformAnim = GetComponent<Animator>();
    }

    
    void Update()
    {
        Move();
    }

    void Move()
    {
        var step = speed * Time.deltaTime;

        if (waitCountdown > 0)
        {
            waitCountdown -= Time.deltaTime;
            platformAnim.SetBool("isMoving", false);
        }
        else
        {

            if (Vector3.Distance(transform.position, pointOne.position) < 0.01f)
            {
                if (isAtPointOne)
                {
                    target = pointTwo.position;
                }
                else
                {
                    isAtPointOne = true;
                    waitCountdown = waitTime;
                }
            }
            else if (Vector3.Distance(transform.position, pointTwo.position) < 0.01f)
            {
                if (isAtPointOne)
                {
                    isAtPointOne = false;
                    waitCountdown = waitTime;
                }
                else
                {
                    target = pointOne.position;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, target, step);
            platformAnim.SetBool("isMoving", true);

        }
    }
}
