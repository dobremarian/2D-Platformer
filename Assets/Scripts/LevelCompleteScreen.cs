using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] Text levelCompleteText;

    [SerializeField] GameObject livesObject;
    [SerializeField] Text livesNumber;
    [SerializeField] Text livesTotalNumber;

    [SerializeField] GameObject hpObject;
    [SerializeField] Text hpNumber;
    [SerializeField] Text hpTotalNumber;

    [SerializeField] GameObject scoreObject;
    [SerializeField] Text scoreNumber;
    [SerializeField] Text scoreTotalNumber;

    [SerializeField] Text totalText;
    [SerializeField] Text totalNumber;

    [SerializeField] Button nextButton;

    GameManager theGM;
    AudioManager theAudioManager;

    bool isLevelComplete = false;
    bool startLevelCompleteCo = false;

    public bool IsLevelComplete
    {
        set { isLevelComplete = value; }
    }
            
    void Awake()
    {
        theGM = GameObject.FindObjectOfType<GameManager>();
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();

        levelCompleteText.gameObject.SetActive(false);

        livesObject.gameObject.SetActive(false);
        livesNumber.gameObject.SetActive(false);
        livesTotalNumber.gameObject.SetActive(false);

        hpObject.gameObject.SetActive(false);
        hpNumber.gameObject.SetActive(false);
        hpTotalNumber.gameObject.SetActive(false);

        scoreObject.gameObject.SetActive(false);
        scoreNumber.gameObject.SetActive(false);
        scoreTotalNumber.gameObject.SetActive(false) ;

        totalText.gameObject.SetActive(false);
        totalNumber.gameObject.SetActive(false) ;

        nextButton.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if(isLevelComplete)
        {
            StartCoroutine(LevelCompleteCo());
        }
    }

    IEnumerator LevelCompleteCo()
    {
        if(!startLevelCompleteCo)
        {
            startLevelCompleteCo = true;

            yield return new WaitForSeconds(1f);
            theAudioManager.PlayLevelComplete();
            levelCompleteText.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);
            theAudioManager.PlaySFX(13);
            livesObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            livesNumber.text = theGM.PlayerLives + " x " + theGM.LivesMultiplyer;
            theAudioManager.PlaySFX(13);
            livesNumber.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            livesTotalNumber.text = "" + Multiply(theGM.PlayerLives, theGM.LivesMultiplyer);
            theAudioManager.PlaySFX(13);
            livesTotalNumber.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.1f);
            theAudioManager.PlaySFX(13);
            hpObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            hpNumber.text = theGM.PlayerHP + " x " + theGM.HpMultiplyer;
            theAudioManager.PlaySFX(13);
            hpNumber.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            hpTotalNumber.text = "" + Multiply(theGM.PlayerHP, theGM.HpMultiplyer);
            theAudioManager.PlaySFX(13);
            hpTotalNumber.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.1f);
            theAudioManager.PlaySFX(13);
            scoreObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            scoreNumber.text = "" + theGM.PlayerScore;
            theAudioManager.PlaySFX(13);
            scoreNumber.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            scoreTotalNumber.text = "" + theGM.PlayerScore;
            theAudioManager.PlaySFX(13);
            scoreTotalNumber.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.1f);
            theAudioManager.PlaySFX(13);
            totalText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            totalNumber.text = "" + theGM.TotalScore;
            theAudioManager.PlaySFX(13);
            totalNumber.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.3f);
            theAudioManager.PlaySFX(12);
            nextButton.gameObject.SetActive(true);

        }
    }

    int Multiply(int variable, float multiplyer)
    {
        return Mathf.RoundToInt(variable * multiplyer);
    }
}
