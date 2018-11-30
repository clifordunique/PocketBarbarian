using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeySymbols : MonoBehaviour {

    public Sprite squareSymbol;
    public Sprite circleSymbol;
    public Sprite triangleSymbol;
    
    
	public void InitKeySymbol(CollectableKeys.KEY_TYPE keyType) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (keyType == CollectableKeys.KEY_TYPE.CIRCLE) {
            sr.sprite = circleSymbol;
        }
        if (keyType == CollectableKeys.KEY_TYPE.SQUARE) {
            sr.sprite = squareSymbol;
        }
        if (keyType == CollectableKeys.KEY_TYPE.TRIANGLE) {
            sr.sprite = triangleSymbol;
        }
    }
}
