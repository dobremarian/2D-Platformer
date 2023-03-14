using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text livesText;
    [SerializeField] Text hpText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Animator fadeScreen;

    GameManager theGM;
    AudioManager theAudioManager;

    void Start()
    {
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        UpdateLives();
        UpdateHP();
        UpdateScore();
        fadeScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !theGM.IsGamePaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && theGM.IsGamePaused)
        {
            UnpauseGame();
        }

    }

    public void UpdateLives()
    {
        livesText.text = theGM.PlayerLives.ToString();
    }

    public void UpdateHP()
    {
        hpText.text = theGM.PlayerHP.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = theGM.PlayerScore.ToString();
    }

    public void FadeToBlack()
    {
        fadeScreen.SetTrigger("FadeToB_T");
    }

    public void FadeFromBlack()
    {
        fadeScreen.SetTrigger("FadeFromB_T");
    }

    public void SetFadeScreenActive()
    {
        fadeScreen.gameObject.SetActive(true);
    }

    public void SetFadeScreenInactive()
    {
        fadeScreen.gameObject.SetActive(false);
    }

    void PauseGame()
    {
        theAudioManager.PlaySFX(6);
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        theGM.IsGamePaused = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        theAudioManager.PlaySFX(7);
        pauseMenu.SetActive(false);
        gameUI.SetActive(true);
        theGM.IsGamePaused = false;
        Time.timeScale = 1;
    }

    public void SaveGame()
    {

    }

    public void QuitGame()
    {

    }
}
