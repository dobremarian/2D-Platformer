using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [SerializeField] string currentSceneName;
    [SerializeField] string levelSelectSceneName;
    [SerializeField] string mainMenuSceneName;
    [SerializeField] string gameCompleteSceneName;

    [SerializeField] Text livesText;
    [SerializeField] Text hpText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Animator fadeScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject levelCopleteScreen;

    PlayerController thePlayer;
    GameManager theGM;
    AudioManager theAudioManager;
    LevelSavingManager theLSM;
    [SerializeField] AudioVolumeManager theVolumeManager;

    bool isGameOver;
    bool isLevelComplete;
    bool gameOverCoStart = false;
    bool levelCompleteCoStart = false;
    bool canUsePauseMenu = false;

    public bool IsGameOver
    {
        set { isGameOver = value; }
    }
    public bool IsLevelComplete
    {
        set { isLevelComplete = value; }
    }

    public bool CanUsePauseMenu
    {
        set { canUsePauseMenu = value; }
    }

    void Start()
    {
        thePlayer = GameObject.FindObjectOfType<PlayerController>();
        theGM = GameObject.FindObjectOfType<GameManager>();
        theLSM = GameObject.FindObjectOfType<LevelSavingManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();

        UpdateLives();
        UpdateHP();
        UpdateScore();

        pauseMenu.SetActive(false);

        theVolumeManager.gameObject.SetActive(true);
        theVolumeManager.LoadSliderValues();
        theVolumeManager.gameObject.SetActive(false);
        
        gameOverScreen.SetActive(false);
        levelCopleteScreen.SetActive(false);
        isGameOver = false;
        isLevelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUsePauseMenu)
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

        if (isGameOver)
        {
            StartCoroutine(GameOverScreenCo());
        }

        if (isLevelComplete)
        {
            StartCoroutine(LevelCompleteScreenCo());
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

    public void IdleBlackScreen()
    {
        fadeScreen.SetTrigger("IdleBlack_T");
    }



    void PauseGame()
    {
        FadeScreenSetActive(false);
        theAudioManager.PlaySFX(6);
        pauseMenu.SetActive(true);
        theVolumeManager.gameObject.SetActive(false);
        gameUI.SetActive(false);
        theGM.IsGamePaused = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        FadeScreenSetActive(true);
        theAudioManager.PlaySFX(7);
        pauseMenu.SetActive(false);
        theVolumeManager.SaveSliderValues();
        theVolumeManager.gameObject.SetActive(false);
        gameUI.SetActive(true);
        theGM.IsGamePaused = false;
        Time.timeScale = 1;
    }

    void FadeScreenSetActive(bool value)
    {
        fadeScreen.gameObject.SetActive(value);
        fadeScreen.SetTrigger("Idle_T");
    }

    public void VolumeMenuOpen()
    {
        pauseMenu.SetActive(false);
        theVolumeManager.gameObject.SetActive(true);
    }

    public void VolumeMenuBackButton()
    {
        theVolumeManager.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }

    IEnumerator GameOverScreenCo()
    {
        if (!gameOverCoStart)
        {
            gameOverCoStart = true;
            theAudioManager.StopLevelMusic();
            yield return new WaitForSeconds(1f);
            FadeToBlack();
            yield return new WaitForSeconds(1.5f);
            IdleBlackScreen();
            yield return new WaitForSeconds(0.25f);
            theAudioManager.PlayGameOver();
            gameOverScreen.SetActive(true);
            FadeScreenSetActive(false);
        }
    }

    IEnumerator LevelCompleteScreenCo()
    {
        if (!levelCompleteCoStart)
        {
            levelCompleteCoStart = true;
            theAudioManager.StopLevelMusic();
            yield return new WaitForSeconds(0.75f);
            FadeToBlack();
            yield return new WaitForSeconds(1.5f);
            IdleBlackScreen();
            yield return new WaitForSeconds(0.25f);
            levelCopleteScreen.SetActive(true);
            FadeScreenSetActive(false);
            levelCopleteScreen.gameObject.GetComponent<LevelCompleteScreen>().IsLevelComplete = true;

        }
    }

    public void RestartGame()
    {
        StartCoroutine(LoadSceneCo(currentSceneName));
    }

    public void LoadLevelSelectMenu()
    {
        if (theGM.LevelNumber != theLSM.NumberOfLevels)
        {
            StartCoroutine(LoadSceneCo(levelSelectSceneName));
        }
        else
        {
            StartCoroutine(LoadSceneCo(gameCompleteSceneName));
        }
    }

    public void LoadMainMenu()
    {
            StartCoroutine(LoadSceneCo(mainMenuSceneName));
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        pauseMenu.SetActive(false);
        gameUI.SetActive(false);
        canUsePauseMenu = false;
        FadeScreenSetActive(true);
        thePlayer.CanMove = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.05f);
        FadeToBlack();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
