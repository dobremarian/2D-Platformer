using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : MonoBehaviour
{
    Animator enemyPersonAnim;
    SpriteRenderer enemyPersonSprite;
    Rigidbody2D enemyPersonRb;
    GameManager theGM;
    AudioManager theAudioManager;

    [SerializeField] int hp;
    [SerializeField] int damageToPlayer;
    [SerializeField] int score;

    [SerializeField] Transform leftPoint, rightPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float jumpForce = 8f;
    float waitCountdown;
    float knockbackForce = 0.2f;
    bool isOnGround = false;
    bool isHit = false;
    bool isWaiting;
    bool isRight = true;
    Vector3 target;

    [SerializeField] GameObject desappearEffect;
    [SerializeField] List<GameObject> enemyDrops;
    void Awake()
    {
        enemyPersonAnim = GetComponent<Animator>();
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        enemyPersonSprite = GetComponent<SpriteRenderer>();
        enemyPersonRb = GetComponent<Rigidbody2D>();


        waitCountdown = waitTime;
        isWaiting = true;

        rightPoint.parent = null;
        leftPoint.parent = null;

        target = rightPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!theGM.IsGamePaused)
        {
            MoveEnemy();
            enemyPersonAnim.SetFloat("Jump_F", enemyPersonRb.velocity.y);
        }
    }
    void EnemyJump()
    {
        if(isOnGround)
        {
            isOnGround = false;
            enemyPersonRb.velocity = new Vector2(enemyPersonRb.velocity.x, jumpForce);
            enemyPersonAnim.SetBool("isOnGround_B", false);
        }
        
    }

    IEnumerator TakeDamageCo()
    {
        isHit = true;
        hp--;
        enemyPersonAnim.SetTrigger("Hit_T");
        if (!isWaiting)
        {
            if (isRight)
            {
                enemyPersonRb.velocity = new Vector2(-knockbackForce, enemyPersonRb.velocity.y);
            }
            else
            {
                enemyPersonRb.velocity = new Vector2(knockbackForce, enemyPersonRb.velocity.y);
            }
        }
        if (hp == 0)
        {
            theAudioManager.PlaySFX(4);
            Destroy(gameObject);
            Instantiate(desappearEffect, gameObject.transform.position, desappearEffect.transform.rotation);
            int rand = UnityEngine.Random.Range(0, enemyDrops.Count);
            Instantiate(enemyDrops[rand], gameObject.transform.position, enemyDrops[rand].transform.rotation);
            theGM.PlayerScore += score;
            theGM.UpdateStats();
        }
        yield return new WaitForSeconds(0.2f);
        isHit = false;
    }

    void MoveEnemy()
    {
        if (waitCountdown > 0)
        {
            waitCountdown -= Time.deltaTime;
        }
        else
        {
            //enemyPersonAnim.SetBool("Moving_B", true);
            isWaiting = false;
            if (Vector3.Distance(transform.position, rightPoint.transform.position) < 0.5f)
            {
                
                //enemyPersonAnim.SetBool("Moving_B", false);
                if (isRight && isOnGround)
                {
                    target = leftPoint.transform.position;
                    waitCountdown = waitTime;
                    isWaiting = true;
                    enemyPersonSprite.flipX = true;
                    isRight = false;
                }

            }
            else if (Vector3.Distance(transform.position, leftPoint.transform.position) < 0.5f)
            {

                
                //enemyPersonAnim.SetBool("Moving_B", false);
                if (!isRight && isOnGround)
                {
                    target = rightPoint.transform.position;
                    waitCountdown = waitTime;
                    isWaiting = true;
                    enemyPersonSprite.flipX = false;
                    isRight = true;
                }
            }
            if (!isHit && !isWaiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
                EnemyJump();
            }

        }

    }

    public void EnemyFrogTriggerHit()
    {
        StartCoroutine(TakeDamageCo());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToPlayer);
            enemyPersonAnim.SetTrigger("Hit_T");
            StartCoroutine(TakeDamageCo());
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            enemyPersonAnim.SetBool("isOnGround_B", true);
        }
    }


}
