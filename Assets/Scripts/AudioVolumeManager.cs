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
    //string fileName = "AudioSliderValues.gd";
    //string dirName = "Pink-Guy_Game_Files";
    string filePath = "/AudioSliderValues.gd";
    string path = "";

    AudioSliderValues sliderValues;

    [SerializeField] Slider gameSlider, musicSlider, sfxSlider;

    void Awake()
    {
        sliderValues = new AudioSliderValues();
        //filePath = Path.Combine(dirName, fileName);

        //string path = Application.persistentDataPath + filePath;
        path = Application.persistentDataPath + filePath;
        
        Debug.Log(path);

        if (File.Exists(path))
        {
            LoadSliderValues();
        }
        else
        {
            this.masterValue = this.musicValue = this.sfxValue = 1f;
            SaveSliderValues();
            //LoadSliderValues();
        }

    }

    public void SetGameVolume(float sliderValue)
    {
        volumeMixer.SetFloat("MasterVolume", MathF.Log10(sliderValue) * 20);
        this.masterValue = sliderValue;
    }

    public void SetMusicVolume(float sliderValue)
    {
        volumeMixer.SetFloat("MusicVolume", MathF.Log10(sliderValue) * 20);
        this.musicValue = sliderValue;
    }

    public void SetSFXVolume(float sliderValue)
    {
        volumeMixer.SetFloat("SFXVolume 1", MathF.Log10(sliderValue) * 20);
        volumeMixer.SetFloat("SFXVolume 2", MathF.Log10(sliderValue) * 20);

        this.sfxValue = sliderValue;
    }

    public void SaveSliderValues()
    {
        sliderValues.MasterValue = this.masterValue;
        sliderValues.MusicValue = this.musicValue;
        sliderValues.SfxValue = this.sfxValue;

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
        //string path = Application.persistentDataPath + filePath;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, values);
        file.Close();

    }

    void LoadValuesFile()
    {
        //string path = Application.persistentDataPath + filePath;
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
        set{ sfxValue = value; }
    }

    public AudioSliderValues()
    {
        masterValue = musicValue = sfxValue = 1f;
    }

}
