using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource[] music;
    [SerializeField] AudioSource[] soundEffects;
    [SerializeField] AudioSource shootSfx;
    [SerializeField] AudioSource levelComplete, gameOver;

    public void PlayShootSFX()
    {
        shootSfx.Stop();
        shootSfx.Play();
    }

    public void PlaySFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();
        soundEffects[soundToPlay].pitch = Random.Range(.95f, 1.15f);
        soundEffects[soundToPlay].Play();
    }

    public void PlayLevelComplete()
    {
        levelComplete.Stop();
        levelComplete.Play();
    }

    public void PlayGameOver()
    {
        gameOver.Stop();
        gameOver.Play();
    }


}
