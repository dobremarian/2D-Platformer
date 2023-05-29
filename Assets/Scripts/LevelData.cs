using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    int levelNumber;
    bool isComplete;
    bool isUnlocked;
    int levelScore;

    public int LevelNumber
    {
        get { return levelNumber; }
    }
    public bool IsComplete
    {
        get { return isComplete; }
        set { isComplete = value; }
    }

    public bool IsUnlocked
    {
        get { return isUnlocked; }
        set { isUnlocked = value; }
    }

    public int LevelScore
    {
        get { return levelScore; }
        set { levelScore = value; }
    }

    public LevelData(int level)
    {
        levelNumber = level;
        isComplete = false;
        isUnlocked = false;
        levelScore = 0;
    }


}
