using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject continueButton, newGameButton;
    [SerializeField] GameObject pauseMenu, volumeMenu;
    LevelSavingManager theLSM;
    LevelData levelOneData;
    AudioVolumeManager theAVM;
    //AudioManager theAM;

    [SerializeField] Animator fadeScreen;

    [SerializeField] string levelOneName, levelSelectMenuName;

    void Start()
    {
        volumeMenu.SetActive(true);
        theAVM = FindObjectOfType<AudioVolumeManager>();
        theAVM.LoadSliderValues();
        volumeMenu.SetActive(false);
        //theAM = FindObjectOfType<AudioManager>();
        

        theLSM = FindObjectOfType<LevelSavingManager>();
        
        levelOneData = theLSM.GetLevelData(1);

        if(!levelOneData.IsComplete)
        {
            continueButton.SetActive(false);
        }
        pauseMenu.SetActive(true);

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

    void FadeScreenSetActive(bool value)
    {
        fadeScreen.gameObject.SetActive(value);
        fadeScreen.SetTrigger("Idle_T");
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        //pauseMenu.SetActive(false);
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
        yield return new WaitForSeconds(0.05f);
        FadeFromBlack();
        yield return new WaitForSeconds(1f);
        //pauseMenu.SetActive(true);
        yield return new WaitForSeconds(1f); ;
        FadeScreenSetActive(false);
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
}
