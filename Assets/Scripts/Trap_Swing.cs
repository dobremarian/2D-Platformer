using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Trap_Swing : Trap
{
    [SerializeField] float zSwingAngleValue;
    [SerializeField] float swingSpeed;
    bool isRight = false;

    [SerializeField] Animator platformAnim;

    protected override void Start()
    {
        base.Start();
        platformAnim.SetTrigger("RotRight_T");
        
    }

    
    void Update()
    {
        if(transform.localEulerAngles.z >= zSwingAngleValue && transform.localEulerAngles.z < 180f)
        {
            isRight = true;
            platformAnim.SetTrigger("RotLeft_T");
        }
        else if(transform.localEulerAngles.z <= 360f - zSwingAngleValue && transform.localEulerAngles.z > 180f)
        {
            isRight = false;
            platformAnim.SetTrigger("RotRight_T");
        }

        if(isRight)
        {
            //target = new Vector3(0, 0, -zSwingValue);
            transform.Rotate(-Vector3.forward * Time.deltaTime * swingSpeed, Space.World);
        }
        else
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * swingSpeed, Space.World);
        }


    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
