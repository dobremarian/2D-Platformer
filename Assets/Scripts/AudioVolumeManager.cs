using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AudioVolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer volumeMixer;
    float masterValue, musicValue, sfxValue;
    string filePath = "/AudioSliderValues.gd";

    AudioSliderValues sliderValues;

    [SerializeField] Slider gameSlider, musicSlider, sfxSlider;

    void Awake()
    {
        LoadSliderValues();
    }

    public void SetGameVolume(float sliderValue)
    {
        volumeMixer.SetFloat("MasterVolume", MathF.Log10(sliderValue) * 20);
        masterValue = sliderValue;
    }

    public void SetMusicVolume(float sliderValue)
    {
        volumeMixer.SetFloat("MusicVolume", MathF.Log10(sliderValue) * 20);
        musicValue = sliderValue;
    }

    public void SetSFXVolume(float sliderValue)
    {
        volumeMixer.SetFloat("SFXVolume 1", MathF.Log10(sliderValue) * 20);
        volumeMixer.SetFloat("SFXVolume 2", MathF.Log10(sliderValue) * 20);

        sfxValue = sliderValue;
    }

    public void SaveSliderValues()
    {
        sliderValues.MasterValue = masterValue;
        sliderValues.MusicValue = musicValue;
        sliderValues.SfxValue = sfxValue;

        SaveValuesFile();
    }

    public void LoadSliderValues()
    {
        LoadValuesFile();

        gameSlider.value = sliderValues.MasterValue;
        SetGameVolume(sliderValues.MasterValue);
        
        musicSlider.value = sliderValues.MusicValue;
        SetMusicVolume(sliderValues.MusicValue);

        sfxSlider.value = sliderValues.SfxValue;
        SetSFXVolume(sliderValues.SfxValue);
    }

    void SaveValuesFile()
    {
        AudioSliderValues values = sliderValues;
        string path = Application.dataPath + filePath;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, values);
        file.Close();

    }

    void LoadValuesFile()
    {
        string path = Application.dataPath + filePath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            sliderValues = (AudioSliderValues)bf.Deserialize(file);
            file.Close();
        }
    }
}

[System.Serializable]
class AudioSliderValues
{
    float masterValue, musicValue, sfxValue;

    public float MasterValue
    {
        get { return masterValue; }
        set { masterValue = value; }
    }

    public float MusicValue
    {
        get { return musicValue; }
        set { musicValue = value; }
    }

    public float SfxValue
    {
        get { return sfxValue; }
        set
        {
            sfxValue = value;
        }
    }

}
