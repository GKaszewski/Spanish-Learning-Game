using UnityEngine;
using System.Collections;
using System;

public class AFKDamage : MonoBehaviour {
    private Action triggerHint;
    private float sinceLastMovementTime = 0f;
    private float currentAfkAllowedTime = 10f;
    private bool isHintShown = false;

    public float minAfkAllowedTime = 2.5f;
    public float afkAllowedTime = 10f;

    public PlayerHealth playerHealth;
    public float damage = 5f;

    public GameObject hintUI;

    private void OnEnable() {
        triggerHint += ShowHint;
    }

    private void OnDisable() {
        triggerHint -= ShowHint;
    }

    private void Update() {
        if (GameManager.isMoving) {
            currentAfkAllowedTime = afkAllowedTime;
            sinceLastMovementTime = 0f;
        } else sinceLastMovementTime += Time.deltaTime;
        
        if (sinceLastMovementTime > currentAfkAllowedTime) {
            if (!isHintShown) triggerHint?.Invoke();
            playerHealth.TakeDamage(damage);
            sinceLastMovementTime = 0f;
            currentAfkAllowedTime /= 2f;
            if (currentAfkAllowedTime < minAfkAllowedTime) currentAfkAllowedTime = minAfkAllowedTime;
        }
    }

    private void ShowHint() {
        isHintShown = true;
        LeanTween.cancel(hintUI);
        LeanTween.moveLocal(hintUI, new Vector3(0f, -313f, 0f), 1.25f)
            .setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(hintUI, Vector3.one, 1.25f)
            .setEase(LeanTweenType.easeOutElastic);
        StartCoroutine(HideHintUI());
    }

    private IEnumerator HideHintUI() {
        yield return new WaitForSeconds(2f);
        LeanTween.cancel(hintUI);
        LeanTween.moveLocal(hintUI, new Vector3(0f, -813f, 0f), 0.75f)
                   .setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(hintUI, Vector3.zero, 0.75f)
            .setEase(LeanTweenType.easeInElastic).setOnComplete(() => {
                isHintShown = false;
            });
    }
}
