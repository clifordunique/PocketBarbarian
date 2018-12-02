using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashingEffect {

    float flashIntervalDamage = 0.08f;
    float flashIntervalAction = 0.12f;
    Shader shaderFlash;
    Shader shaderDefault;


    public SpriteFlashingEffect() {
        this.shaderFlash = Shader.Find("GUI/Text Shader"); 
        this.shaderDefault = Shader.Find("Sprites/Default");
    }


    void whiteSprite(SpriteRenderer spriteRenderer) {
        spriteRenderer.material.shader = shaderFlash;
        Color myColor = new Color32(246, 214, 189, 255);
        spriteRenderer.color = myColor;
        
    }

    void invisibleSprite(SpriteRenderer spriteRenderer) {
        spriteRenderer.material.shader = shaderDefault;
        spriteRenderer.color = Color.white;
        Color c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;
    }
    void normalSprite(SpriteRenderer spriteRenderer) {
        spriteRenderer.material.shader = shaderDefault;
        spriteRenderer.color = Color.white;
    }

    public IEnumerator ActionFlashing(SpriteRenderer spriteRenderer, float time) {
        float flashCount = Mathf.Round(time / flashIntervalAction);
        for (int i = 0; i < flashCount; i++) {
            // switch color
            if (i % 2 == 0) {
                invisibleSprite(spriteRenderer);
            }
            if (i % 2 == 1) {
                normalSprite(spriteRenderer);
            }
            yield return new WaitForSeconds(flashIntervalAction);
        }
        normalSprite(spriteRenderer);
    }

    public IEnumerator DamageFlashing(SpriteRenderer spriteRenderer, float time) {
        float flashCount = Mathf.Round(time / flashIntervalDamage);
        for (int i = 0; i < flashCount; i++) {
            // switch color
            if (i % 2 == 0) {
                whiteSprite(spriteRenderer);
            }
            if (i % 2 == 1) {
                normalSprite(spriteRenderer);
            }
            yield return new WaitForSeconds(flashIntervalDamage);
        }
        normalSprite(spriteRenderer);
    }
}
