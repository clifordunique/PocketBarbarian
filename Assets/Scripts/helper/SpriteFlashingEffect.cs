using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlashingEffect {

    float flashInterval = 0.08f;
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
    

    public IEnumerator DamageFlashing(SpriteRenderer spriteRenderer, float time) {
        float flashCount = Mathf.Round(time / flashInterval);
        for (int i = 0; i < flashCount; i++) {
            // switch color
            if (i % 2 == 0) {
                whiteSprite(spriteRenderer);
            }
            if (i % 2 == 1) {
                normalSprite(spriteRenderer);
            }
            yield return new WaitForSeconds(flashInterval);
        }
        normalSprite(spriteRenderer);
    }
}
