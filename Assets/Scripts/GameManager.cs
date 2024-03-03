using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] int levelNumber;
    PlayerController thePlayer;
    int playerLives = 1;
    int playerHP = 10;
    int playerScore = 0;
    float livesMultiplyer = 16;
    float hpMultiplyer = 1.4f;
    int totalScore;

    bool canTakeDamage = false;
    bool isGamePaused;
    //bool isLevelComplete;

    UIManager theUiManager;
    LevelSavingManager theLSM;

    public int LevelNumber
    {
        get { return levelNumber; }
    }
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

    public float LivesMultiplyer
    {
        get { return livesMultiplyer; }
    }

    public float HpMultiplyer
    {
        get { return hpMultiplyer; }
    }

    public int TotalScore
    {
        get { return totalScore; }
    }

    public bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    /*
    public bool IsLevelComplete
    {
        get { return isLevelComplete; }
        set { isLevelComplete = value; }
    }*/



    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<PlayerController>();
        theUiManager = GameObject.FindObjectOfType<UIManager>();
        theLSM = GameObject.FindObjectOfType<LevelSavingManager>();
        canTakeDamage = true;
        isGamePaused = false;
        StartCoroutine(LevelStartCo());
        //isLevelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        ConvertHP();
        /*
        if (isLevelComplete)
        {
        thePlayer.IsLevelComplete = true;
            canTakeDamage = false;
            theUiManager.IsLevelComplete = true;
            totalScore = Multiply(playerLives, livesMultiplyer) + Multiply(playerHP, hpMultiplyer) + playerScore;
        }
        */
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
        //theUiManager.SetFadeScreenActive();
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
        theUiManager.IsGameOver = true;

        UpdateStats();
    }

    IEnumerator LevelStartCo()
    {
        thePlayer.CanMove = false;
        yield return new WaitForSeconds(0.5f);
        theUiManager.StartLevel();
        theUiManager.FadeFromBlack();
        thePlayer.IsSpawning = true;
        yield return new WaitForSeconds(1.5f);
        theUiManager.CanUsePauseMenu = true;
        thePlayer.CanMove = true;
    }

    public void LevelComplete()
    {
        thePlayer.IsLevelComplete = true;
        canTakeDamage = false;
        theUiManager.IsLevelComplete = true;
        totalScore = Multiply(playerLives, livesMultiplyer) + Multiply(playerHP, hpMultiplyer) + playerScore;
        theLSM.UpdateLevelData(levelNumber, totalScore);
    }

    public void UpdateStats()
    {
        theUiManager.UpdateLives();
        theUiManager.UpdateHP();
        theUiManager.UpdateScore();
    }

    int Multiply(int variable, float multiplyer)
    {
        return Mathf.RoundToInt(variable * multiplyer);
    }

}
