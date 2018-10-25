using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour {

    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SelectRandomSprite();
	}
	
	private void SelectRandomSprite() {
        int index = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[index];
    }
}
