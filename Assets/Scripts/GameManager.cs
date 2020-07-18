using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

[Serializable]
public enum GameMode{
    SPANISH_TO_ENGLISH ,
    ENGLISH_TO_SPANISH,
}

[Serializable]
public enum DifficultyLevel{
    EASY = 0,
    MEDIUM = 1,
    HARD = 2,
    HARDCORE = 3
}

public class GameManager : MonoBehaviour{
    [SerializeField]    
    public static List<Word> words = new List<Word>();
    public static string Path {get;set;} 

    public static int Points { get;set;} = 0;
    public static GameMode gameMode = GameMode.SPANISH_TO_ENGLISH;
    public static DifficultyLevel difficulty = DifficultyLevel.EASY;
    public static bool hasHints = false;

    public static GameData data = new GameData();

    private void Awake() {
        Path = $"{Application.dataPath}/StreamingAssets/spanish words.json";
        UpdateDictionary.OnDownloadingDone += () => {
            LoadWordsFromJSON();
            Debug.Log($"Words count: {words.Count}");
        };
    }
    
    private  void LoadWordsFromJSON(){
        string content = "";
        using(var reader = new StreamReader(Path)){
            content = reader.ReadToEnd();
        }

        var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
        foreach (var wordDict in result){
            var word = new Word(wordDict["English"], wordDict["Spanish"]);
            words.Add(word);
        }
    }

    public static void Save(){
        if(data == null) data = new GameData();
        
        data.difficulty = difficulty;
        data.gameMode = gameMode;
        data.points = Points;
        SaveSystem.SaveData(data);
    }

    public static void Load() {
        data = SaveSystem.LoadData();
        if(data == null)
            return;
        Points = data.points;
        gameMode = data.gameMode;
        difficulty = data.difficulty;
    }

    public static void SaveSettings(){
        data.difficulty = difficulty;
        data.gameMode = gameMode;
        SaveSystem.SaveData(data);
    }
}