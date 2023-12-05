using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMask : MonoBehaviour
{

    Animator enemyPersonAnim;
    SpriteRenderer enemyPersonSprite;
    Rigidbody2D enemyPersonRb;
    GameManager theGM;
    AudioManager theAudioManager;
    Transform playerTr;

    [SerializeField] int hp;
    [SerializeField] int damageToPlayer;
    [SerializeField] int score;

    [SerializeField] Transform leftPoint, rightPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float yPlayerRange = 1f;
    float knockbackForce = 5f;
    float waitCountdown;
    //bool isOnGround = false;
    bool canBeDamaged;
    bool playerInRange;
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
        playerTr = GameObject.FindObjectOfType<PlayerController>().GetComponent<Transform>();


        waitCountdown = waitTime;
        isWaiting = true;
        canBeDamaged = false;
        playerInRange = false;

        rightPoint.parent = null;
        leftPoint.parent = null;

        transform.position = leftPoint.transform.position;

        target = rightPoint.transform.position;

        enemyPersonAnim.SetFloat("Speed_F", moveSpeed);
        enemyPersonAnim.SetBool("Moving_B", false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerTr.position.x < rightPoint.position.x && playerTr.position.x > leftPoint.position.x) && ((playerTr.position.y < (rightPoint.position.y + yPlayerRange)) && (playerTr.position.y > (leftPoint.position.y - yPlayerRange))))
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
            enemyPersonAnim.SetBool("Moving_B", false);
        }
        if (!theGM.IsGamePaused && playerInRange)
        {
            MoveEnemy();
        }
    }
    /*void EnemyJump()
    {
        if (isOnGround)
        {
            isOnGround = false;
            enemyPersonRb.velocity = new Vector2(enemyPersonRb.velocity.x, jumpForce);
            enemyPersonAnim.SetBool("isOnGround_B", false);
        }

    }*/

    IEnumerator TakeDamageCo()
    {
        isHit = true;
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
        enemyPersonAnim.SetTrigger("Hit_T");

        if (canBeDamaged)
        {
            hp--;
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
        }

        yield return new WaitForSeconds(1.2f);
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
            enemyPersonAnim.SetBool("Moving_B", true);
            isWaiting = false;
            canBeDamaged = false;
            if (Vector3.Distance(transform.position, rightPoint.transform.position) < 0.01f)
            {

                enemyPersonAnim.SetBool("Moving_B", false);
                if (isRight)
                {
                    target = leftPoint.transform.position;
                    waitCountdown = waitTime;
                    canBeDamaged = true;
                    isWaiting = true;
                    enemyPersonSprite.flipX = true;
                    isRight = false;
                }

            }
            else if (Vector3.Distance(transform.position, leftPoint.transform.position) < 0.01f)
            {


                enemyPersonAnim.SetBool("Moving_B", false);
                if (!isRight)
                {
                    target = rightPoint.transform.position;
                    waitCountdown = waitTime;
                    canBeDamaged = true;
                    isWaiting = true;
                    enemyPersonSprite.flipX = false;
                    isRight = true;
                }
            }
            if (!isHit && !isWaiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            }
        }


    }

    public void EnemyMaskTriggerHit()
    {
        if (canBeDamaged)
        {
            StartCoroutine(TakeDamageCo());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            theGM.DamagePlayer(damageToPlayer);
            StartCoroutine(TakeDamageCo());
        }
    }

}
