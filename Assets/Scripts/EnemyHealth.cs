using UnityEngine;
using System.Collections;

public class EnemyHealth : Health{    
    public AudioClip deathSound;
    public GameObject deathEffect;
    public override void TakeDamage(float damage = 10f)
    {
        int pointsToAdd = GameManager.hasHints ? 5 : 10;
        var particles = Instantiate(deathEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(particles, 1.5f);
        GameManager.Guessed++;
        GameManager.Points += pointsToAdd;
        GameManager.AllEnemies--;
        gameObject.SetActive(false);
    }
}
