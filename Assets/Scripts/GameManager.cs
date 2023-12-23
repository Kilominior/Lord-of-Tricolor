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

    public enum GameState
    {
        WELCOME,
        WON,
        LOST
    }

    // 当前轮次，限制在1~9999之间，修改时更新最大轮次
    private static int currentRound;
    public static int CurrentRound
    {
        get { return currentRound; }
        set {
            currentRound = value;
            Mathf.Clamp(currentRound, 1, 9999);
            if (currentRound > maxRound) MaxRound = currentRound;
        }
    }

    // 当前分值，限制在0~999999之间，修改时更新最大分值
    private static int currentScore;
    public static int CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value;
            Mathf.Clamp(currentScore, 0, 999999);
            if(currentScore > maxScore) MaxScore = currentScore;
        }
    }

    // 游戏是否开始
    public static bool GameStarted;

    // 游戏目前的状态，只对游戏未开始的情况有效
    public static GameState currentState;

    // 球目前的状态，true对应的默认状态为吸收者
    public static bool isGameModeNormal;

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

    private static Hardness gameHardness;
    public static Hardness GameHardness
    {
        set { gameHardness = value; GameSave(); }
        get { return gameHardness; }
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
        gameHardness = saveData.SavedHardness;
        Debug.Log("<GameManager>: 读取现有存档");

        CurrentRound = 1;
        CurrentScore = 0;
        GameStarted = false;
        isGameModeNormal = true;
        currentState = GameState.WELCOME;
    }

    public static void SaveDataReset()
    {
        MaxRound = 0;
        MaxScore = 0;
        gameHardness = Hardness.EASY;
    }

    private static void GameSave()
    {
        if (saveData == null)
        {
            saveData = new SaveData(MaxRound, MaxScore, gameHardness);
        }
        else saveData.GameSave(MaxRound, MaxScore, gameHardness);

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