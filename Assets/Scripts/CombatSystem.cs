using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CombatSystem : MonoBehaviour {
    private float timeScaleFactor = 0.25f;
    private Word word = new Word();
    private GameObject currentEnemy;
    private EnemyWord enemyWord;
    private bool isResponding = false;
    private Transform camTransform;
    private FpsController fpsController;
    private Ray ray;
    private DepthOfField blur = null;
    private HashSet<string> usedWords = new HashSet<string>();

    public LayerMask targetLayer;
    public GameObject responseTool;
    public GameObject crosshair;
    public GameObject bgOverlay;
    public TMP_InputField textField;
    public TMP_Text wordText;
    public TMP_Text hintText;
    public TMP_Text hintsLabel;
    public TMP_Text pointsText;
    public Volume postVolume;

    private void Awake() {
        camTransform = Camera.main.transform;
        fpsController = GetComponent<FpsController>();
        responseTool.SetActive(false);

        hintText.enabled = GameManager.hasHints;
        hintsLabel.enabled = GameManager.hasHints;

        postVolume.profile.TryGet<DepthOfField>(out blur);
        DisableBlur();
    }

    private void Update() {
        switch(GameManager.difficulty){
            case DifficultyLevel.EASY:
                timeScaleFactor = 0.1f;
                break;
            case DifficultyLevel.MEDIUM:
                timeScaleFactor = 0.25f;
                break;
            case DifficultyLevel.HARD:
                timeScaleFactor = 0.7f;
                break;
            case DifficultyLevel.HARDCORE:
                timeScaleFactor = 1f;
                break;
        }

        ray = new Ray(camTransform.position, camTransform.forward);

        pointsText.text = $"Points: {GameManager.Points}";

        Raycasting();
    }

    private void Raycasting() {
        if (Input.GetMouseButtonDown(0) && !isResponding) {
            if (Physics.Raycast(ray, out var hit, 100f, targetLayer)) {
                if (hit.collider.CompareTag("Enemy")) {
                    currentEnemy = hit.collider.gameObject;
                    StartAttackingSequence();
                }
            }
        }
    }

    private void StartAttackingSequence()
    {
        if (GameManager.words.Count == 0)
            return;

        isResponding = true;

        int wordIndex = Random.Range(0, GameManager.words.Count+1);
        word = GameManager.words[wordIndex];
        while (usedWords.Contains(word.English)) {
            wordIndex = Random.Range(0, GameManager.words.Count + 1);
            word = GameManager.words[wordIndex];
        }

        enemyWord = currentEnemy.GetComponent<EnemyWord>();
        if (enemyWord) {
            if (enemyWord.Word == null) enemyWord.SetWord(word);
            else word = enemyWord.Word;
        }

        usedWords.Add(word.English);

        if (GameManager.hasHints) {
            switch (GameManager.gameMode) {
                case GameMode.SPANISH_TO_ENGLISH:
                    wordText.text = word.Spanish;
                    var wordLength = word.English.Length;
                    hintText.text = word.English.Substring(0, Random.Range(3, wordLength-1));
                    break;
                case GameMode.ENGLISH_TO_SPANISH:
                    wordText.text = word.English;
                    hintText.text = word.Spanish.Substring(0, 3);;
                    break;
            }
        }
        else {
            switch (GameManager.gameMode) {
                case GameMode.SPANISH_TO_ENGLISH:
                    wordText.text = word.Spanish;
                    break;
                case GameMode.ENGLISH_TO_SPANISH:
                    wordText.text = word.English;
                    break;
            }
        }
       

        ShowAttackingStuff();
        textField.onValueChanged.AddListener(OnTextFieldChange);
    }

    private void ShowAttackingStuff()
    {
        crosshair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fpsController.enabled = false;
        Time.timeScale = timeScaleFactor;
        EnableBlur();
        responseTool.SetActive(true);
        bgOverlay.SetActive(true);
    }

    private void HideAttackingStuff()
    {
        bgOverlay.SetActive(false);
        responseTool.SetActive(false);
        DisableBlur();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.SetActive(true);
        fpsController.enabled = true;
        textField.text = "";
        isResponding = false;
    }

    private bool CheckTranslation()
    {
        var translation = textField.text;

        if(GameManager.gameMode == GameMode.SPANISH_TO_ENGLISH){
            if (translation.ToLower() == word.English.ToLower())
                return true;
        }else if(GameManager.gameMode == GameMode.ENGLISH_TO_SPANISH){
             if (translation.ToLower() == word.Spanish.ToLower())
                return true;
        }

        return false;
    }

    private void OnTextFieldChange(string text){
        if (CheckTranslation())
        {
            HideAttackingStuff();
            DoDamage();
            enemyWord.SetWord(null);
        }
    }

    private void DoDamage()
    {
        var health = currentEnemy.GetComponent<EnemyHealth>();
        if(health != null){
            health.TakeDamage();
        }
    }

    public void Cancel() => HideAttackingStuff();

    public void Skip() {
        int pointsToRemove = GameManager.hasHints ? 5 : 10;
        GameManager.Points -= pointsToRemove;
        GameManager.Skipped++;
        enemyWord.SetWord(null);
        HideAttackingStuff();
    }

    private void EnableBlur() {
        if (!blur) return;
        blur.active = true;
    }

    private void DisableBlur() {
        if (!blur) return;
        blur.active = false;
    }
}