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

    public enum Hardness
    {
        EASY,
        HARD
    }

    public static int CurrentRound;

    // 当前分值，限制在0~999999之间
    private static int currentScore;
    public static int CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value; Mathf.Clamp(currentScore, 0, 999999); }
    }

    // 游戏是否开始
    public static bool GameStarted;
    // 游戏目前的状态，true对应的默认状态为吸收者
    public static bool isGameStateNormal;

    private static int maxRound;
    public static int MaxRound
    {
        set { maxRound = value; GameSave(); }
        get { return maxRound; }
    }

    private static int maxScore;
    public static int MaxScore
    {
        set { maxScore = value; GameSave(); }
        get { return maxScore; }
    }

    private static Hardness currentHardness;
    public static Hardness CurrentHardness
    {
        set { currentHardness = value; GameSave(); }
        get { return currentHardness; }
    }

    static GameManager()
    {
        if (!File.Exists(Application.persistentDataPath + "/GameSave.sav"))
        {
            SaveDataReset();
            Debug.Log("<GameManager>: 创建新存档");
            return;
        }

        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Open(Application.persistentDataPath + "/GameSave.sav", FileMode.Open);
        saveData = BF.Deserialize(FS) as SaveData;
        FS.Close();
        maxRound = saveData.MaxRound;
        maxScore = saveData.MaxScore;
        currentHardness = saveData.SavedHardness;
        Debug.Log("<GameManager>: 读取现有存档");

        GameRestart();
    }

    public static void GameRestart()
    {
        CurrentRound = 1;
        CurrentScore = 0;
        GameStarted = false;
        isGameStateNormal = true;
    }

    public static void SaveDataReset()
    {
        MaxRound = 0;
        MaxScore = 0;
        currentHardness = Hardness.EASY;
    }

    private static void GameSave()
    {
        if (saveData == null)
        {
            saveData = new SaveData(MaxRound, MaxScore, currentHardness);
        }
        else saveData.GameSave(MaxRound, MaxScore, currentHardness);

        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Create(Application.persistentDataPath + "/GameSave.sav");
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

    public GameManager.Hardness SavedHardness { private set; get; }

    public SaveData(int MaxRound, int MaxScore, GameManager.Hardness savedHardness)
    {
        this.MaxRound = MaxRound;
        this.MaxScore = MaxScore;
        SavedHardness = savedHardness;
    }

    public void GameSave(int MaxRound, int MaxScore, GameManager.Hardness savedHardness)
    {
        this.MaxRound = MaxRound;
        this.MaxScore = MaxScore;
        SavedHardness = savedHardness;
    }
}