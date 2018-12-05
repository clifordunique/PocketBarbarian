using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class SecretTrigger : MonoBehaviour {

    public LayerMask playerMask;
    public Tilemap overlay;
    public float fadeDuration;
    

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (playerMask == (playerMask | (1 << collider.gameObject.layer))) {
            StartCoroutine(Fade(fadeDuration, 0));
        }
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (playerMask == (playerMask | (1 << collider.gameObject.layer))) {
            StartCoroutine(Fade(fadeDuration, 1));
        }
    }

    private IEnumerator Fade(float duration, float endAlpha) {
        Color color = overlay.color;
        float startAlpha = color.a;

        for (float t = 0; t < duration; t += Time.deltaTime) {
            color.a = Mathf.SmoothStep(startAlpha, endAlpha, t / duration);
            overlay.color = color;
            yield return null;
        }
        color.a = endAlpha;
        overlay.color = color;
    }
}
