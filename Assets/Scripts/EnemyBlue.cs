using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : MonoBehaviour
{
    Animator enemyPersonAnim;
    SpriteRenderer enemyPersonSprite;
    Rigidbody2D enemyPersonRb;
    GameManager theGM;
    AudioManager theAudioManager;

    [SerializeField] int hp;
    [SerializeField] int damageToPlayer;
    [SerializeField] int score;
    [SerializeField] int projectileDamage;
    [SerializeField] float fireRate = 0.4f;

    [SerializeField] Transform leftPoint, rightPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime = 2f;
    float waitCountdown;
    float knockbackForce = 2.1f;
    bool isHit = false;
    bool isShooting = false;
    bool isWaiting; 
    bool isRight = true;
    Vector3 target;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform firepointTR;
    float xFireOffset = 0.15f;
    float projectileSpeed = 3f;


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
        }
    }

    void Fire()
    {
        //float fireForce;
        Vector2 direction;

        if (isRight)
        {
            direction = rightPoint.transform.position - gameObject.transform.position;
        }
        else
        {
            direction = leftPoint.transform.position - gameObject.transform.position;
        }
        //theAudioManager.PlayShootSFX();
        var projectile_ = Instantiate(projectile, firepointTR.position, firepointTR.rotation);
        projectile_.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;
        projectile_.GetComponent<EnemyProjectile>().DamageToPlayer = projectileDamage;
    }

    IEnumerator ShootCo()
    {
        isShooting = true;
        Fire();
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    IEnumerator TakeDamageCo()
    {
        isHit = true;
        hp--;
        enemyPersonAnim.SetTrigger("Hit_T");
        if(!isWaiting)
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
            if(!isShooting)
            {
                StartCoroutine(ShootCo());
            }
        }
        else
        {
            enemyPersonAnim.SetBool("Moving_B", true);
            isWaiting = false;
            if (Vector3.Distance(transform.position, rightPoint.transform.position) < 0.01f)
            {
                target = leftPoint.transform.position;
                waitCountdown = waitTime;
                isWaiting = true;
                enemyPersonAnim.SetBool("Moving_B", false);
                if (isRight)
                {
                    enemyPersonSprite.flipX = true;
                    firepointTR.position = new Vector3(-xFireOffset + gameObject.transform.position.x, firepointTR.position.y, firepointTR.position.z);
                    isRight = false;
                }

            }
            else if (Vector3.Distance(transform.position, leftPoint.transform.position) < 0.01f)
            {

                target = rightPoint.transform.position;
                waitCountdown = waitTime;
                isWaiting = true;
                enemyPersonAnim.SetBool("Moving_B", false);
                if (!isRight)
                {
                    enemyPersonSprite.flipX = false;
                    firepointTR.position = new Vector3(xFireOffset + gameObject.transform.position.x, firepointTR.position.y, firepointTR.position.z);
                    isRight = true;
                }
            }
            if(!isHit)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            }

        }
        
    }

    public void EnemyBlueTriggerHit()
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
    }


}
