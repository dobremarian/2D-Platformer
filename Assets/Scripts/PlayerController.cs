using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float moveSpeed = 3f;
    private float jumpForce = 8f;
    private float knockbackForce = 2.1f;
    private float knockbackTime = 0.3f;
    private float knockbackCounter;
    private bool isAlive;
    private bool isSpawning;
    private bool isRespawning;
    private bool isDamaged;
    private bool isGrounded = true;
    private bool isFlying = false;
    private bool canDoubleJump = true;
    private bool canMove = false;
    private bool isFacingRight = true;
    private bool isLevelComplete = false;
    bool startDeathCo;
    bool startSpawnCo;
    bool startRespawnCo;
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    private Animator playerAnim;

    [SerializeField] GameObject projectile;
    private Transform firepointTR;
    private float xFireOffset = 0.15f;
    private float projectileSpeed = 6.5f;


    private Vector3 screenPosition;
    private Vector3 worldPosition;

    private GameManager theGM;
    AudioManager theAudioManager;
    CheckpointController theCheckPointCtrl;

    public bool CanMove
    {
        set { canMove = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

    public bool IsSpawning
    {
        set { isSpawning = value; }
    }

    public bool IsRespawning
    {
        get { return isRespawning; }
        set { isRespawning = value; }
    }

    public bool IsGrounded
    {
        //get { return isGrounded; }
        set { isGrounded = value; }
    }

    public bool CanDoubleJump
    {
        //get { return canDoubleJump; }
        set { canDoubleJump = value; }
    }

    public bool IsDamaged
    {
        set { isDamaged = value; }
    }

    public bool IsLevelComplete
    {
        set { isLevelComplete = value; }
    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        firepointTR = gameObject.transform.Find("Fire Point").GetComponent<Transform>();
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        theCheckPointCtrl = GameObject.FindObjectOfType<CheckpointController>();

        isAlive = true;
        isRespawning = false;
        startSpawnCo = false;
        startDeathCo = false;
        startRespawnCo = false;
        canMove = false;
    }


    void Update()
    {
        if (!theGM.IsGamePaused)
        {
            FollowMouse();
            if (canMove)
            {
                PlayerMove();
                PlayerJump();
                if (Input.GetMouseButtonDown(0))
                {
                    Fire();
                }
            }

            if (!isAlive)
            {
                canMove = false;
                StartCoroutine(PlayerDeathCo());

            }

            if(isSpawning)
            {
                canMove = false;
                StartCoroutine(PlayerSpawnCo());
            }

            if(isRespawning)
            {
                canMove = false;
                StartCoroutine(PlayerRespawnCo());
            }

            if(isLevelComplete)
            {
                canMove = false;
            }

            if (isDamaged)
            {
                theAudioManager.PlaySFX(2);
                knockbackCounter -= Time.deltaTime;
                playerSR.material.color = new Color(playerSR.material.color.r, playerSR.material.color.g, playerSR.material.color.b, 0.4f);
                if (isFacingRight)
                {
                    playerRB.velocity = new Vector2(-knockbackForce, playerRB.velocity.y);
                }
                else
                {
                    playerRB.velocity = new Vector2(knockbackForce, playerRB.velocity.y);
                }

                if (knockbackCounter <= 0)
                {

                    theGM.CanTakeDamage = true;
                    isDamaged = false;
                    playerSR.material.color = new Color(playerSR.material.color.r, playerSR.material.color.g, playerSR.material.color.b, 1f);

                    if (isAlive && !isRespawning)
                    {
                        canMove = true;
                        playerAnim.Play("Player_Idle");
                    }
                }
            }
        }

    }

    void PlayerMove()
    {
        playerRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), playerRB.velocity.y);
        playerAnim.SetFloat("Speed_F", playerRB.velocity.x);
        playerAnim.SetFloat("Jump_F", playerRB.velocity.y);
        if (playerRB.velocity.x < 0)
        {
            playerSR.flipX = true;
            firepointTR.position = new Vector3(-xFireOffset + gameObject.transform.position.x, firepointTR.position.y, firepointTR.position.z);
            isFacingRight = false;
        }
        else if (playerRB.velocity.x > 0)
        {
            playerSR.flipX = false;
            firepointTR.position = new Vector3(xFireOffset + gameObject.transform.position.x, firepointTR.position.y, firepointTR.position.z);
            isFacingRight = true;
        }
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && !isFlying)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);

            if (isGrounded)
            {
                theAudioManager.PlaySFX(0);
                canDoubleJump = true;
                isGrounded = false;
            }
            else
            {
                theAudioManager.PlaySFX(1);
                playerAnim.SetTrigger("JumpDouble_T");
                canDoubleJump = false;
            }
        }
    }

    void Fire()
    {
        //float fireForce;
        Vector2 direction = worldPosition - gameObject.transform.position;

        if (isFacingRight)
        {
            //fireForce = projectileSpeed;
            if (worldPosition.x < gameObject.transform.position.x)
            {
                direction = Vector2.right;
            }
        }
        else
        {
            //fireForce = -projectileSpeed;
            if (worldPosition.x > gameObject.transform.position.x)
            {
                direction = Vector2.left;
            }
        }
        theAudioManager.PlayShootSFX();
        var projectile_ = Instantiate(projectile, firepointTR.position, firepointTR.rotation);
        projectile_.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

    }

    void FollowMouse()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    public void DamagePlayer()
    {
        knockbackCounter = knockbackTime;
        canMove = false;
        theGM.CanTakeDamage = false;
        playerAnim.SetTrigger("Hit_T");
        isDamaged = true;
    }


    IEnumerator PlayerDeathCo()
    {
        if (!startDeathCo)
        {
            theAudioManager.PlaySFX(9);
            startDeathCo = true;
            yield return new WaitForSeconds(0.1f);
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            playerAnim.Play("Player_Desappear");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }

    IEnumerator PlayerSpawnCo()
    {
        if(!startSpawnCo)
        {
            startSpawnCo = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.position = theCheckPointCtrl.SpawnPoint;
            //theAudioManager.PlaySFX(8);
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            playerAnim.Play("Player_Appear");
            yield return new WaitForSeconds(0.6f);//respawn time for animation
            playerRB.constraints = RigidbodyConstraints2D.None;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRB.AddForce(Vector2.down);
            theAudioManager.PlaySFX(8);
            yield return new WaitForSeconds(0.1f);
            canMove = true;
            startSpawnCo = false;
            isSpawning = false;
        }
    }

    IEnumerator PlayerRespawnCo()
    {
        if(!startRespawnCo)
        {
            startRespawnCo = true;
            theAudioManager.PlaySFX(9);
            yield return new WaitForSeconds(0.2f);
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            playerAnim.Play("Player_Desappear");
            yield return new WaitForSeconds(0.4f);//death time for animation
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(1.6f);//fade to black time
            gameObject.transform.position = theCheckPointCtrl.SpawnPoint;//spawn pos
            yield return new WaitForSeconds(1.6f);//fade from black time
            theAudioManager.PlaySFX(8);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            playerAnim.Play("Player_Appear");
            yield return new WaitForSeconds(0.6f);//respawn time for animation
            playerRB.constraints = RigidbodyConstraints2D.None;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRB.AddForce(Vector2.down);
            yield return new WaitForSeconds(0.1f);
            isRespawning = false;
            canMove = true;
            startRespawnCo = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.transform.CompareTag("Platform"))
        {
            transform.parent = other.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.transform.CompareTag("Platform"))
        {
            transform.parent = null;
        }
    }

    public void EnterFan()
    {
            canDoubleJump = false;
            isGrounded = false;
            isFlying = true;
            playerAnim.SetBool("inFan", true);
            playerAnim.Play("Player_Jump");
            
    }

    public void ExitFan()
    {
        playerAnim.SetBool("inFan", false);
        isFlying = false;
        playerAnim.Play("Player_Idle");
    }
}




