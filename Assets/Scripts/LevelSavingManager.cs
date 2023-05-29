using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelSavingManager : MonoBehaviour
{
    [SerializeField] int numberOfLevels;
    List<LevelData> levelsList = new List<LevelData>();
    string filePath = "/LevelSavings.gd";

    void Awake()
    {
        string path = Application.dataPath + filePath;

        if (File.Exists(path))
        {
            LoadLevelsDataFile();
        }
        else
        {
            NewSaveList();
        }
    }

    public void NewSaveList()
    {
        levelsList.Clear();

        for (int i = 0; i < numberOfLevels; i++)
        {
            levelsList.Add(new LevelData(i + 1));
        }
        levelsList[0].IsUnlocked = true;

        SaveLevelsDataFile();
    }

    public void UpdateLevelData(int levelNuber, int score)
    {
        levelsList[levelNuber - 1].IsComplete = true;
        levelsList[levelNuber - 1].LevelScore = score;

        if(!levelsList[levelNuber].IsUnlocked && levelNuber != levelsList.Count)
        {
            levelsList[levelNuber].IsUnlocked = true;
        }

        SaveLevelsDataFile();
    }

    public LevelData GetLevelData(int levelNuber)
    {
        return levelsList[levelNuber - 1];
    }

    public void SaveLevelsDataFile()
    {
        List<LevelData> list = levelsList;
        string path = Application.dataPath + filePath;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, list);
        file.Close();

    }

    public void LoadLevelsDataFile()
    {
        string path = Application.dataPath + filePath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            levelsList = (List<LevelData>)bf.Deserialize(file);
            file.Close();
        }
    }
}
