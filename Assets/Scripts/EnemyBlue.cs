using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlue : MonoBehaviour
{
    Animator enemyPersonAnim;
    SpriteRenderer enemyPersonSprite;


    protected GameManager theGM;
    [SerializeField] int hp;
    [SerializeField] protected int damageToPlayer;
    [SerializeField] int score;
    [SerializeField] List<GameObject> enemyDrops;
    [SerializeField] Transform leftPoint, rightPoint;
    [SerializeField] float waitTime = 2f;

    float waitCountdown;
    bool isRight = true;
    Vector3 target;

    AudioManager theAudioManager;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform firepointTR;
    private float xFireOffset = 0.15f;
    private float projectileSpeed = 6.5f;

    [SerializeField] float moveSpeed;
    void Awake()
    {
        enemyPersonAnim = GetComponent<Animator>();
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        enemyPersonSprite = GetComponent<SpriteRenderer>();


        waitCountdown = waitTime;

        rightPoint.parent = null;
        leftPoint.parent = null;

        target = rightPoint.transform.position;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (!theGM.IsGamePaused)
        {
            MoveEnemy();
        }
    }

    virtual protected void MoveEnemy()
    {
        if (waitCountdown > 0)
        {
            waitCountdown -= Time.deltaTime;
        }
        else
        {

            if (Vector3.Distance(transform.position, rightPoint.transform.position) < 0.01f)
            {
                target = leftPoint.transform.position;
                waitCountdown = waitTime;
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
                enemyPersonAnim.SetBool("Moving_B", false);
                if (!isRight)
                {
                    enemyPersonSprite.flipX = false;
                    firepointTR.position = new Vector3(xFireOffset + gameObject.transform.position.x, firepointTR.position.y, firepointTR.position.z);
                    isRight = true;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
        }
        
    }


}
