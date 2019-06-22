using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health{
    private AudioSource src;
    public float health = 100f;
    public AudioClip hurtSound;

    private void Awake() => src = GetComponent<AudioSource>();
    public override void TakeDamage(){
        health -= 10f;
        src.PlayOneShot(hurtSound);

        if(health <= 0){
            GameManager.Save();
            SceneManager.LoadSceneAsync(2);
            Debug.Log("Game Over");
        }
    }
}