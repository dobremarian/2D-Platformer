using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelSavingManager : MonoBehaviour
{
    [SerializeField] int numberOfLevels;
    List<LevelData> levelsList = new List<LevelData>();
    //string fileName = "LevelSavings.gd";
    //string dirName = "PinkGuy_Game_Files";
    string filePath = "/LevelSavings.gd";
    string path = "";

    public int NumberOfLevels
    {
        get { return numberOfLevels; }
    }

    void Awake()
    {   
        //filePath = Path.Combine(dirName, fileName);

        //string path = Application.persistentDataPath + filePath;
        path = Application.persistentDataPath + filePath;

        //Debug.Log(path);

        if (File.Exists(path))
        {
            LoadLevelsDataFile();
        }
        else
        {
            NewSaveList();
        }
        //Debug.Log(levelsList.Count);
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

        if(levelNuber != levelsList.Count)
        {
            if (!levelsList[levelNuber].IsUnlocked)
            {
                levelsList[levelNuber].IsUnlocked = true;
            }
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
        //string path = Application.persistentDataPath + filePath;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, list);
        file.Close();

    }

    public void LoadLevelsDataFile()
    {
        //string path = Application.persistentDataPath + filePath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            levelsList = (List<LevelData>)bf.Deserialize(file);
            file.Close();
        }
    }
}
