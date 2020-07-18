using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour{

    private bool isPaused = false;
    private FpsController playerController;
    private CombatSystem combatSystem;
    public GameObject pausePanel;
    public Slider difficultySlider;
    public Toggle hints;
    public GameObject settingsPanel;
    public GameObject menuPanel;

    public TMP_Text pointsText;

    private void Awake(){
        if (hints) {
            hints.isOn = GameManager.hasHints;
        }
        playerController = FindObjectOfType<FpsController>();
        combatSystem = FindObjectOfType<CombatSystem>();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && pausePanel != null && !combatSystem.responseTool.activeSelf) {
            Pause();
        }

        if(isPaused){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(pointsText != null){
            pointsText.text = $"POINTS: {GameManager.Points}";
        }
    }

    private void Pause() {
        if (isPaused) {
            SetControl(true);
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;
            isPaused = false;
        }
        else {
            SetControl(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            isPaused = true;
        }
    }

    private void SetControl(bool on) {
        if (playerController != null)
            playerController.enabled = on;

        if (combatSystem != null)
            combatSystem.enabled = on;
    }

    public void StartGame(){
        GameManager.Load();
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame(){
        GameManager.Save();
        Application.Quit();
    }

    public void Resume() {
        isPaused = false;
        if(playerController != null)
            playerController.enabled = true;

        if(combatSystem != null)
            combatSystem.enabled = true;

        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void SaveGame() => GameManager.Save();

    public void LoadGame() => GameManager.Load();

    public void SetDifficulty(){
        GameManager.difficulty = (DifficultyLevel) difficultySlider.value;
        Debug.Log($"Current difficulty: {GameManager.difficulty}");
    }

    public void SetHints() {
        GameManager.hasHints = hints.isOn;
    }

    public void ShowSettings(){
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void HideSettings(){
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
        GameManager.SaveSettings();
    }

    public void GoToMenu(){
        GameManager.Save();
        SceneManager.LoadSceneAsync(0);
    }

    public void SpanishToEnglish(){
        GameManager.gameMode = GameMode.SPANISH_TO_ENGLISH;
    }

    public void EnglishToSpanish(){
        GameManager.gameMode = GameMode.ENGLISH_TO_SPANISH;
    }

    public void GoToMenuWithoutSaving(){
        SceneManager.LoadSceneAsync(0);
    }
}