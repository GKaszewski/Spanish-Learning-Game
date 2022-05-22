using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIController : MonoBehaviour {
    private bool isPaused = false;
    private FpsController playerController;
    private CombatSystem combatSystem;
    private DepthOfField blur = null;

    public Slider difficultySlider;
    public Toggle hints;
    public GameObject settingsPanel;
    public GameObject menuPanel;
    public GameObject pausePanel;
    public GameObject bgOverlay;
    public GameObject exitButton;

    public TMP_Text pointsText;
    public TMP_Text highScoreText;
    public Volume postVolume;

    private void Awake(){
        LoadGame();
        if (hints) {
            hints.isOn = GameManager.hasHints;
        }

        if (difficultySlider) {
            difficultySlider.value = (int)GameManager.difficulty;
        }

        playerController = FindObjectOfType<FpsController>();
        combatSystem = FindObjectOfType<CombatSystem>();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        postVolume.profile.TryGet(out blur);

#if UNITY_WEBGL
        exitButton.SetActive(false);
#endif
    }

    private void Update(){
#if UNITY_STANDALONE
if(Input.GetKeyDown(KeyCode.Escape) && pausePanel != null && !combatSystem.responseTool.activeSelf) {
            Pause();
        }
#endif
#if UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.P) && pausePanel != null && !combatSystem.responseTool.activeSelf) {
            Pause();
        }
#endif


        if (isPaused){
            ShowMouse();
        }

        if(pointsText){
            pointsText.text = $"POINTS: {GameManager.Points}";
        }

        if (highScoreText) {
            highScoreText.text = $"HIGHSCORE: {GameManager.HighScore}";
        }
    }

    private void EnablePause() {
        SetControl(false);
        ShowMouse();
        Time.timeScale = 0f;
        EnableBlur();
        pausePanel.SetActive(true);
        bgOverlay.SetActive(true);
        isPaused = true;
    }
    private void DisablePause() {
        SetControl(true);
        DisableBlur();
        pausePanel.SetActive(false);
        bgOverlay.SetActive(false);
        HideMouse();
        Time.timeScale = 1f;
        isPaused = false;
    }


    private void HideMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void ShowMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Pause() {
        if (isPaused) {
            DisablePause();
        } else {
            EnablePause();
        }
    }

    private void SetControl(bool on) {
        if (playerController != null)
            playerController.enabled = on;

        if (combatSystem != null)
            combatSystem.enabled = on;
    }

    public void StartGame(){
        GameManager.Points = 0;
        GameManager.Load();
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame(){
        GameManager.Save();
        Application.Quit();
    }

    public void Resume() {
        DisablePause();
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

    private void EnableBlur() {
        if (!blur) return;
        blur.active = true;
    }

    private void DisableBlur() {
        if (!blur) return;
        blur.active = false;
    }
}