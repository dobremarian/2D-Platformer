using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected GameManager theGM;
    [SerializeField] int hp;
    [SerializeField] protected int damageToPlayer;
    [SerializeField] int score;
    bool isHit;
    string direction;

    Animator anim;
    float blinkTime = 3.5f;
    bool itHasBlinked = false;

    AudioManager theAudioManager;


    [SerializeField] GameObject topCol;
    [SerializeField] GameObject bottomCol;
    [SerializeField] GameObject leftCol;
    [SerializeField] GameObject rightCol;
    [SerializeField] GameObject desappearEffect;
    [SerializeField] Transform pointToFollow;
    [SerializeField] List<GameObject> enemyDrops;
    Vector3 initialPosition;
    Vector3 pointToFollowPosition;
    Vector3 target;
    [SerializeField] float moveDownSpeed = 2f;
    [SerializeField] float moveUpSpeed = 2f;
    [SerializeField] float waitTime = 2f;
    float moveSpeed;
    float waitCountdown;
    bool isInitialPos = true;



    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();

        topCol.gameObject.GetComponent<EnemyHitbox>().Direction = "top";
        bottomCol.gameObject.GetComponent<EnemyHitbox>().Direction = "bottom";
        leftCol.gameObject.GetComponent<EnemyHitbox>().Direction = "left";
        rightCol.gameObject.GetComponent<EnemyHitbox>().Direction = "right";

        theGM = GameObject.FindObjectOfType<GameManager>();
        initialPosition = gameObject.transform.position;
        pointToFollowPosition = pointToFollow.position;

        theAudioManager = GameObject.FindObjectOfType<AudioManager>();

        waitCountdown = waitTime;
    }
    void Update()
    {
        if (!theGM.IsGamePaused)
        {
            EnemyHit();
            if (!itHasBlinked)
            {
                StartCoroutine(BlinkAnimationCo());

            }

            MoveEnemy();
            
        }
    }

    public void HitboxHit(string direction)
    {
        isHit = true;
        this.direction = direction;

    }

    void EnemyHit()
    {
        if (isHit)
        {
            if (direction == "top")
            {
                anim.SetTrigger("TopHit_T");
            }
            if (direction == "bottom")
            {
                anim.SetTrigger("BottomHit_T");
            }
            if (direction == "left")
            {
                anim.SetTrigger("LeftHit_T");
            }
            if (direction == "right")
            {
                anim.SetTrigger("RightHit_T");
            }
        }
        isHit = false;
    }

    public void TakeDamage()
    {
        hp--;
        if (hp == 0)//if the enemy hp somehow goes under 0 and doesn't trigger this, then the enemy taking damage sistem should change
        {
            theAudioManager.PlaySFX(4);
            Destroy(gameObject);
            Instantiate(desappearEffect, gameObject.transform.position, desappearEffect.transform.rotation);
            int rand = UnityEngine.Random.Range(0, enemyDrops.Count);
            Instantiate(enemyDrops[rand], gameObject.transform.position, enemyDrops[rand].transform.rotation);
            theGM.PlayerScore += score;
            theGM.UpdateStats();
        }
    }

    IEnumerator BlinkAnimationCo()
    {
        if (!itHasBlinked)
        {
            itHasBlinked = true;
            anim.SetTrigger("Blink_T");
            yield return new WaitForSeconds(blinkTime);
            itHasBlinked = false;
        }
    }

    void MoveEnemy()
    {
        if (waitCountdown > 0)
        {
            waitCountdown -= Time.deltaTime;
        }
        else
        {
            
            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                if(isInitialPos)
                {
                    target = pointToFollowPosition;
                    moveSpeed = moveDownSpeed;
                }
                else
                {
                    isInitialPos = true;
                    waitCountdown = waitTime;
                }
            }
            else if (Vector3.Distance(transform.position, pointToFollowPosition) < 0.01f)
            {
                if(isInitialPos)
                {
                    isInitialPos = false;
                    waitCountdown = waitTime;
                }
                else
                {
                    target = initialPosition;
                    moveSpeed = moveUpSpeed;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

        }
    }

}
