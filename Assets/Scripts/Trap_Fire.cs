using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : Trap
{
    [SerializeField] float fireOnTime = 2f;
    float fireDelayTime = 1f;
    bool isFireOn = false;
    bool canDealDamage = false;
    bool isInsideFire = false;

    Animator fireAnim;

    protected override void Start()
    {
        base.Start();
        fireAnim = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        if(isInsideFire && canDealDamage)
        {
            theGM.DamagePlayer(damageToDeal);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            isInsideFire = false;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            isInsideFire = true;
            if(!isFireOn)
            {
                fireAnim.SetTrigger("SwitchOn_T");
                StartCoroutine(FireCo());
                isFireOn = true;
            }
        }
    }


    IEnumerator FireCo()
    {
        yield return new WaitForSeconds(fireDelayTime);
        fireAnim.SetTrigger("FireOn_T");
        canDealDamage = true;
        yield return new WaitForSeconds(fireOnTime);
        fireAnim.SetTrigger("SwitchOff_T");
        isFireOn = false;
        canDealDamage = false;
    }
}
