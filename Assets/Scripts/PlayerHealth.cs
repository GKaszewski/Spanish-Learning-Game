using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : Health{
    private AudioSource src;
    
    public float health = 100f;
    public AudioClip hurtSound;
    public TMP_Text healthText;

    private void Awake() => src = GetComponent<AudioSource>();
    public override void TakeDamage(float damage = 10f){
        health -= damage;
        healthText.SetText($"{health}%");
        src.PlayOneShot(hurtSound);

        if(health <= 0){
            GameManager.Save();
            SceneManager.LoadSceneAsync(2);
            Debug.Log("Game Over");
        }
    }
}