using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    //[SerializeField] GameObject newGameButton;
    [SerializeField] GameObject pauseMenu, volumeMenu;
    LevelSavingManager theLSM;
    LevelData levelOneData;
    AudioVolumeManager theAVM;
    //AudioManager theAM;

    [SerializeField] Animator fadeScreen;

    [SerializeField] string levelOneName, levelSelectMenuName;

    void Start()
    {
        theLSM = FindObjectOfType<LevelSavingManager>();
        theAVM = FindObjectOfType<AudioVolumeManager>();
                
        StartCoroutine(StartSceneCo());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(volumeMenu.activeInHierarchy)
            {
                theAVM.SaveSliderValues();
                CloseVolumeMenu();
            }
        }
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

    void FadeScreenSetActive()
    {
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.SetTrigger("Idle_T");
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        //pauseMenu.SetActive(false);
        FadeScreenSetActive(); //testBuild
        yield return new WaitForSeconds(0.05f);
        FadeToBlack(); //testBuild
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator StartSceneCo()
    {
        //FadeScreenSetActive(true);
        pauseMenu.SetActive(false);

        levelOneData = theLSM.GetLevelData(1);
        if (!levelOneData.IsComplete)
        {
            continueButton.SetActive(false);
        }

        volumeMenu.gameObject.SetActive(true);
        theAVM.LoadSliderValues(); //testBuild
        volumeMenu.gameObject.SetActive(false);

        pauseMenu.SetActive(true);

        //FadeScreenSetActive(true); //testBuild

        //yield return new WaitForSeconds(0.05f);
        FadeFromBlack(); //testBuild
        yield return new WaitForSeconds(2f);
        //pauseMenu.SetActive(true);
        //yield return new WaitForSeconds(1f);
        fadeScreen.gameObject.SetActive(false);
        //FadeScreenSetActive(false); //testBuild
    }

    public void NewGame()
    {
        theLSM.NewSaveList();
        StartCoroutine(LoadSceneCo(levelOneName));
        //SceneManager.LoadScene(levelOneName);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneCo(levelSelectMenuName));
        //SceneManager.LoadScene(levelSelectMenuName);
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
