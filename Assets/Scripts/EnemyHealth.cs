using UnityEngine;
using System.Collections;

public class EnemyHealth : Health{    
    public AudioClip deathSound;
    public GameObject deathEffect;
    public override void TakeDamage()
    {
        var particles = Instantiate(deathEffect, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(particles, 1.5f);
        GameManager.Points += 10;
        gameObject.SetActive(false);
    }
}
