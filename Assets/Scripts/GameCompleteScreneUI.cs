using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleteScreneUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenuButton;
    [SerializeField] Animator fadeScreen;
    [SerializeField] string sceneToLoad;

    private PlayerControllerGCS thePlayerGCS;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerGCS = GameObject.FindObjectOfType<PlayerControllerGCS>();
        thePlayerGCS.IsSpawning= true;

        StartCoroutine(StartSceneCo());
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }
    */
    void FadeToBlack()
    {
        fadeScreen.SetTrigger("FadeToB_T");
    }

    void FadeFromBlack()
    {
        fadeScreen.SetTrigger("FadeFromB_T");
    }

    void FadeScreenSetActive(bool value)
    {
        fadeScreen.gameObject.SetActive(value);
        fadeScreen.SetTrigger("Idle_T");
    }

    IEnumerator LoadSceneCo(string sceneName)
    {
        FadeScreenSetActive(true);
        yield return new WaitForSeconds(0.05f);
        FadeToBlack();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator StartSceneCo()
    {
        yield return new WaitForSeconds(0.05f);
        FadeFromBlack();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        FadeScreenSetActive(false);
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneCo(sceneToLoad));
    }

    public void HoverMainMenuButton(bool isHovered)
    {
        if(isHovered)
        {
            thePlayerGCS.CanFire = false;
        }
        else
        {
            thePlayerGCS.CanFire = true;
        }
    }
}
