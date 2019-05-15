using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutlineEffect {

    float flashInterval = 0.1F;//0.08f;
    Material defaultMaterial;
    Material outlineMaterial;


    public SpriteOutlineEffect(Material defaultMaterial, Material outlineMaterial) {
        this.defaultMaterial = defaultMaterial; 
        this.outlineMaterial = outlineMaterial;
    }


    void whiteSprite(SpriteRenderer spriteRenderer) {
        spriteRenderer.material = outlineMaterial;
        
    }
    
    void normalSprite(SpriteRenderer spriteRenderer) {
        spriteRenderer.material = defaultMaterial;
    }
    

    public IEnumerator OutlineFlashing(SpriteRenderer spriteRenderer, float time) {
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
