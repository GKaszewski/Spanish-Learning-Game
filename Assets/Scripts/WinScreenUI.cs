using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreenUI : MonoBehaviour {
    public TMP_Text pointsText;
    public TMP_Text highscoreText;
    public TMP_Text guessedRightText;
    public TMP_Text skippedText;

    private void Start() {
        pointsText.text = $"POINTS: {GameManager.Points}";
        highscoreText.text = $"HIGHSCORE: {GameManager.HighScore}";
        skippedText.text = $"SKIPPED: {GameManager.Skipped}";
        guessedRightText.text = $"GUESSED: {GameManager.Guessed}";
        ShowMouse();
    }

    public void GoToMenu() {
        GameManager.Save();
        SceneManager.LoadSceneAsync(0);
    }

    private void ShowMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
