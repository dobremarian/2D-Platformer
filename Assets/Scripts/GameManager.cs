using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController thePlayer;
    int playerLives = 0;
    int playerHP = 4;
    int playerScore = 0;
    bool canTakeDamage = false;
    bool isGamePaused;

    UIManager theUiManager;

    public bool CanTakeDamage
    {
        get { return canTakeDamage; }
        set { canTakeDamage = value; }
    }

    public int PlayerLives
    {
        get { return playerLives; }
        set { playerLives = value; }
    }

    public int PlayerHP
    {
        get { return playerHP; }
        set { playerHP = value; }
    }

    public int PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }



    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<PlayerController>();
        theUiManager = GameObject.FindObjectOfType<UIManager>();
        canTakeDamage = true;
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        ConvertHP();
    }

    void ConvertHP()
    {
        if (playerHP >= 20)
        {
            do
            {
                playerHP -= 10;
                playerLives++;
            } while (playerHP > 20);

            UpdateStats();
        }
    }

    public void DamagePlayer(int damage)
    {
        if (canTakeDamage)
        {
            playerHP -= damage;
            thePlayer.DamagePlayer();
            if (playerHP <= 0)
            {
                if (playerLives > 0)
                {
                    playerLives--;
                    playerHP = 10;
                    StartCoroutine(PlayerRespawnCo());
                }
                else
                {
                    PlayerDeath();
                }
            }

            UpdateStats();

        }

    }

    IEnumerator PlayerRespawnCo()
    {
        UpdateStats();
        thePlayer.IsRespawning = true;
        yield return new WaitForSeconds(0.4f);//player death animation time
        theUiManager.SetFadeScreenActive();
        theUiManager.FadeToBlack();
        yield return new WaitForSeconds(1.8f);//fade to black time
        theUiManager.FadeFromBlack();
        yield return new WaitForSeconds(1.8f);//fade from black time

    }

    void PlayerDeath()
    {
        if (playerHP <= 0 && playerLives <= 0)
        {
            playerHP = 0;
            playerLives = 0;
            thePlayer.IsAlive = false;
        }

        UpdateStats();
    }

    public void UpdateStats()
    {
        theUiManager.UpdateLives();
        theUiManager.UpdateHP();
        theUiManager.UpdateScore();
    }

}
