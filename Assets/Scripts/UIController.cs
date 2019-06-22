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
    public GameObject settingsPanel;
    public GameObject menuPanel;

    public TMP_Text pointsText;

    private void Awake(){
        playerController = FindObjectOfType<FpsController>();
        combatSystem = FindObjectOfType<CombatSystem>();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && pausePanel != null){
            if(isPaused){
                if(playerController != null)
                    playerController.enabled = true;

                if(combatSystem != null)
                    combatSystem.enabled = true;

                pausePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1f;
                isPaused = false;
            } else {
                if(playerController != null)
                    playerController.enabled = false;

                if(combatSystem != null)
                    combatSystem.enabled = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = !true;
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
                isPaused = true;
            }
        }

        if(isPaused){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(pointsText != null){
            pointsText.text = $"POINTS: {GameManager.Points}";
        }
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