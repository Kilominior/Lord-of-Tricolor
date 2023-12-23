using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GameManager
{
    public static SaveData saveData;

    public enum Language
    {
        EN,
        CH
    }

    public static int CurrentRound;
    public static int CurrentScore;

    private static int maxRound;
    public static int MaxRound
    {
        set { maxRound = value; GameSave(); }
        get { return maxRound; }
    }

    private static int maxScore;
    public static int MaxScore
    {
        set { MaxScore = value; GameSave(); }
        get { return maxScore; }
    }

    static GameManager()
    {
        CurrentRound = 0;
        CurrentScore = 0;
        if (!File.Exists(Application.persistentDataPath + "/GameSave.sav"))
        {
            MaxRound = 0;
            MaxScore = 0;
            Debug.Log("<GameManager>: 创建新存档");
            return;
        }

        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Open(Application.persistentDataPath + "/GameSave.sav", FileMode.Open);
        saveData = BF.Deserialize(FS) as SaveData;
        FS.Close();
        MaxRound = saveData.MaxRound;
        MaxScore = saveData.MaxScore;
        Debug.Log("<GameManager>: 读取现有存档");
    }

    public static void SaveDataReset()
    {
        MaxRound = 0;
        MaxScore = 0;
    }

    private static void GameSave()
    {
        if (saveData == null)
        {
            saveData = new SaveData(MaxRound, MaxScore);
        }
        else saveData.GameSave(MaxRound, MaxScore);

        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Create(Application.persistentDataPath + "/PTBSave.sav");
        BF.Serialize(FS, saveData);
        FS.Close();
        Debug.Log("<GameManager>: 游戏已保存.");
    }
}

[System.Serializable]
public class SaveData
{
    public int MaxRound { private set; get; }

    public int MaxScore { private set; get; }

    public SaveData(int MaxRound, int MaxScore)
    {
        this.MaxRound = MaxRound;
        this.MaxScore = MaxScore;
    }

    public void GameSave(int MaxRound, int MaxScore)
    {
        this.MaxRound = MaxRound;
        this.MaxScore = MaxScore;
    }
}