using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;

[Serializable]
public enum GameMode{
    SPANISH_TO_ENGLISH = 0,
    ENGLISH_TO_SPANISH = 1,
}

[Serializable]
public enum DifficultyLevel{
    EASY = 0,
    MEDIUM = 1,
    HARD = 2,
    HARDCORE = 3
}

public class GameManager : MonoBehaviour{
    private static string wwwData;
    [SerializeField]    
    public static List<Word> words = new List<Word>();
    public static string PathString {get;set;} 

    public static int Points { get;set;} = 0;
    public static int HighScore { get; set; } = 0;
    public static int Guessed { get; set; } = 0;
    public static int AllEnemies { get; set; }
    public static int Skipped { get; set; } = 0;
    public static GameMode gameMode = GameMode.SPANISH_TO_ENGLISH;
    public static DifficultyLevel difficulty = DifficultyLevel.EASY;
    public static bool hasHints = false;
    public static bool isMoving = false;

    public static GameData data = new GameData();

    private void Awake() {
        if (data == null) data = new GameData();
        PathString = Path.Combine(Application.streamingAssetsPath, "spanish words.json");
        UpdateDictionary.OnDownloadingDone += () => {
            LoadWordsFromJSON();
            Debug.Log($"Words count: {words.Count}");
        };
    }
    

    private IEnumerator GetWordsFromJSONOnWebGL() {
        UnityWebRequest www = UnityWebRequest.Get(PathString);
        yield return www.SendWebRequest();
        wwwData = www.downloadHandler.text;
    }

    private void LoadWordsFromJSON(){
        string content = "";
#if UNITY_WEBGL
        StartCoroutine(GetWordsFromJSONOnWebGL());
        Debug.Log(wwwData);
#endif


        using(var reader = new StreamReader(PathString)){
            content = reader.ReadToEnd();
        }

        var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
        if (result.Count == words.Count) return; 
        foreach (var wordDict in result){
            var word = new Word(wordDict["English"], wordDict["Spanish"]);
            words.Add(word);
        }
    }

    public static void AddWordsToDictionary(List<Dictionary<string, string>> data) {
        if (words.Count == data.Count) return;
        Debug.Log("I am adding data from the API!");
        foreach (var wordDict in data) {
            var word = new Word(wordDict["English"], wordDict["Spanish"]);
            words.Add(word);
        }
    }

    private static void SaveOnStandalone() {
        if (data == null) data = new GameData();

        data.difficulty = difficulty;
        data.highScore = HighScore;
        data.gameMode = gameMode;
        data.hasHints = hasHints;
        SaveSystem.SaveData(data);
    }

    private static void SaveOnWebGL() {
        PlayerPrefs.SetInt("highScore", HighScore);
        SaveSettingsOnWebGL();
    }

    public static void Save(){
        if (Points > HighScore) HighScore = Points;
        #if UNITY_STANDALONE
            SaveOnStandalone();
#endif
#if UNITY_WEBGL
        SaveOnWebGL();
#endif
    }

    private static void LoadOnStandalone() {
        data = SaveSystem.LoadData();
        if (data == null)
            return;
        HighScore = data.highScore;
        gameMode = data.gameMode;
        difficulty = data.difficulty;
        hasHints = data.hasHints;
    }

    private static void LoadOnWebGL() {
        HighScore = PlayerPrefs.GetInt("highScore");
        gameMode = (GameMode)PlayerPrefs.GetInt("gameMode");
        difficulty = (DifficultyLevel)PlayerPrefs.GetInt("difficulty");
        hasHints = Convert.ToBoolean(PlayerPrefs.GetInt("hasHints"));
    }

    public static void Load() {
#if UNITY_STANDALONE
        LoadOnStandalone();
#endif
#if UNITY_WEBGL
        LoadOnWebGL();
#endif
    }

    private static void SaveSettingsOnStandalone() {
        SaveSystem.SaveData(data);
        Debug.Log("Save settings!");
    }

    private static void SaveSettingsOnWebGL() {
        PlayerPrefs.SetInt("difficulty", (int)data.difficulty);
        PlayerPrefs.SetInt("gameMode", (int)data.gameMode);
        PlayerPrefs.SetInt("hasHints", Convert.ToInt32(hasHints));
    }


    public static void SaveSettings(){
        data.difficulty = difficulty;
        data.gameMode = gameMode;
        data.hasHints = hasHints;
#if UNITY_STANDALONE
        SaveSettingsOnStandalone();
#endif
#if UNITY_WEBGL
        SaveSettingsOnWebGL();
#endif
    }

    public static void CalculateHighScore() {
        if (Points > HighScore) HighScore = Points;
    }
}