using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LevelSelectMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Image previewImage;
    [SerializeField] Text scoreText;
    string levelToLoad;

    [SerializeField] LevelSelectButton[] levelButtons;

    [SerializeField] RectTransform buttonHeightRef;
    [SerializeField] RectTransform levelSelectMenuHeightRef;
    [SerializeField] RectTransform gridContentPosYRef;
    [SerializeField] VerticalLayoutGroup verticalLayoutGroupRef;
    [SerializeField] int numberOfButtonsShown;

    float buttonHeight;
    float topPadding, buttonSpacePadding;
    float buttonPadding;

    int listIndex = 0;
    int buttonIndex = 0;

    float mouseScrollWheelImput;
    bool verticalKeyDown;
    float verticaltalImput;

    LevelSavingManager theLSM;
    LevelData currentButtonData;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject volumeMenu;
    bool isPaused;

    bool isLoading = false;
    [SerializeField] Animator fadeScreen;

    [SerializeField] string mainMenuScene;

    void Start()
    {
        theLSM = FindObjectOfType<LevelSavingManager>();

        StartAssignments();
        ResizeMenuWindowHeight();

        isPaused = false;
        pauseMenu.SetActive(false);
        volumeMenu.SetActive(true);
        volumeMenu.GetComponent<AudioVolumeManager>().LoadSliderValues();
        volumeMenu.SetActive(false);

        levelButtons[buttonIndex].SelectButton();
        CurrentButtonUpdateUI();

        StartCoroutine(StartSceneCo());
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLoading)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }

            if (!isPaused)
            {
                mouseScrollWheelImput = Input.mouseScrollDelta.y;
                verticalKeyDown = Input.GetButtonDown("Vertical");
                verticaltalImput = Input.GetAxisRaw("Vertical");

                if (mouseScrollWheelImput > 0 || (verticalKeyDown && verticaltalImput > 0))
                {
                    SwitchSelectedButton("Up");
                }
                else if (mouseScrollWheelImput < 0 || (verticalKeyDown && verticaltalImput < 0))
                {
                    SwitchSelectedButton("Down");
                }
            }
        }
        
    }

    void SwitchSelectedButton(string direction)
    {
        if (direction == "Up")
        {
            if (buttonIndex > 0)
            {
                buttonIndex--;
                ScrollUp();
            }
            else if(listIndex > 0)
            {
                gridContentPosYRef.anchoredPosition = new Vector2(gridContentPosYRef.anchoredPosition.x, gridContentPosYRef.anchoredPosition.y - buttonPadding);
                ScrollUp();
            }
        }
        else if (direction == "Down")
        {
            if (buttonIndex < numberOfButtonsShown - 1)
            {
                buttonIndex++;
                ScrollDown(); 
            }
            else if (listIndex < levelButtons.Length - 1)
            {
                gridContentPosYRef.anchoredPosition = new Vector2(gridContentPosYRef.anchoredPosition.x, gridContentPosYRef.anchoredPosition.y + buttonPadding);
                ScrollDown();
            }
        }

        CurrentButtonUpdateUI();
    }

    void CurrentButtonUpdateUI()
    {
        currentButtonData = theLSM.GetLevelData(listIndex + 1);

        if (currentButtonData.IsUnlocked)
        {
            playButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
        }

        scoreText.text = "Score: " + currentButtonData.LevelScore;
    }

    void ScrollUp()
    {
        levelButtons[listIndex].DeselectButton();
        listIndex--;
        levelButtons[listIndex].SelectButton();
    }

    void ScrollDown()
    {
        levelButtons[listIndex].DeselectButton();
        listIndex++;
        levelButtons[listIndex].SelectButton();
    }

    public void SetPreviewImage(Sprite previewImg)
    {
        previewImage.sprite = previewImg;
    }

    public void SetLevelToLoad(string level)
    {
        levelToLoad = level;
    }

    public void LaunchLevel()
    {
        //SceneManager.LoadScene(levelToLoad);
        StartCoroutine(LoadSceneCo(levelToLoad));
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        volumeMenu.GetComponent<AudioVolumeManager>().SaveSliderValues();
        volumeMenu.SetActive(false);
    }

    public void OpenVolumeMenu()
    {
        pauseMenu.SetActive(false);
        volumeMenu.SetActive(true);
    }

    public void CloseVolumeMenu()
    {
        volumeMenu.SetActive(false);
        pauseMenu.SetActive(true);
        
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneCo(mainMenuScene));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void FadeToBlack()
    {
        fadeScreen.SetTrigger("FadeToB_T");
    }

    void FadeFromBlack()
    {
        fadeScreen.SetTrigger("FadeFromB_T");
    }

    /*void IdleBlackScreen()
    {
        fadeScreen.SetTrigger("IdleBlack_T");
    }*/

    void FadeScreenSetActive(bool value)
    {
        fadeScreen.gameObject.SetActive(value);
        fadeScreen.SetTrigger("Idle_T");
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        //pauseMenu.SetActive(false);
        isLoading = true;
        FadeScreenSetActive(true);
        yield return new WaitForSeconds(0.05f);
        FadeToBlack();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator StartSceneCo()
    {
        //pauseMenu.SetActive(false);
        //FadeScreenSetActive(true);
        isLoading = true;
        yield return new WaitForSeconds(0.05f);
        FadeFromBlack();
        yield return new WaitForSeconds(1f);
        //pauseMenu.SetActive(true);
        yield return new WaitForSeconds(1f); ;
        FadeScreenSetActive(false);
        isLoading = false;
    }

    void StartAssignments()
    {
        buttonHeight = buttonHeightRef.sizeDelta.y;

        topPadding = verticalLayoutGroupRef.padding.top;

        buttonSpacePadding = verticalLayoutGroupRef.spacing;

        buttonPadding = buttonHeight + buttonSpacePadding;
    }

    void ResizeMenuWindowHeight()
    {
        float var = topPadding + (numberOfButtonsShown * buttonPadding);
        levelSelectMenuHeightRef.sizeDelta = new Vector2(levelSelectMenuHeightRef.sizeDelta.x, var);
    }
}
