using UnityEngine;
using TMPro;

public class CombatSystem : MonoBehaviour
{
    private static System.Random rnd = new System.Random();
    private float timeScaleFactor = 0.25f;
    private Word word = new Word();
    private GameObject currentEnemy;
    private bool isResponding = false;
    private Transform camTransform;
    private FpsController fpsController;
    private Ray ray;
    public LayerMask targetLayer;
    public GameObject responseTool;
    public GameObject crosshair;
    public TMP_InputField textField;
    public TMP_Text wordText;
    public TMP_Text pointsText;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        fpsController = GetComponent<FpsController>();
        responseTool.SetActive(false);
    }

    private void Update()
    {
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

    private void Raycasting()
    {
        if (Input.GetMouseButtonDown(0) && !isResponding)
        {
            if (Physics.Raycast(ray, out var hit, 100f, targetLayer))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
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

        int wordIndex = rnd.Next(GameManager.words.Count);
        word = GameManager.words[wordIndex];
        
        if(GameManager.gameMode == GameMode.SPANISH_TO_ENGLISH)
            wordText.text = word.Spanish;
        if(GameManager.gameMode == GameMode.ENGLISH_TO_SPANISH)
            wordText.text = word.English;

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
        responseTool.SetActive(true);
    }

    private void HideAttackingStuff()
    {
        responseTool.SetActive(false);
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

}