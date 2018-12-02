using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiKeys : MonoBehaviour {

    public Sprite keySquare;
    public Sprite keyCircle;
    public Sprite keyTriangle;

    public float offsetPixelsX = 0F;
    public float offsetPixelsY = 0F;
    public float distancePixelsX = 0F;

    public float delayTime = 0.1F;
    private float widthSprite;
    private List<GameObject> currentKeySprites = new List<GameObject>();

    private bool showKeySquare;
    private bool showKeyCircle;
    private bool showKeyTriangle;

    // Use this for initialization
    void Start () {
        widthSprite = keySquare.bounds.size.x;
    }

    public void UpdateKey(CollectableKeys.KEY_TYPE keyType, bool collected) {
        if (keyType == CollectableKeys.KEY_TYPE.SQUARE) {
            showKeySquare = collected;
        }
        if (keyType == CollectableKeys.KEY_TYPE.CIRCLE) {
            showKeyCircle = collected;
        }
        if (keyType == CollectableKeys.KEY_TYPE.TRIANGLE) {
            showKeyTriangle = collected;
        }
        Invoke("UpdateKeys", delayTime);
    }

    public void UpdateKeys() {
        // delete old list of hearts
        foreach (GameObject heart in currentKeySprites) {
            Destroy(heart);
        }
        currentKeySprites.Clear();

        if (showKeySquare) {
            currentKeySprites.Add(InstantiateSprite(keySquare, 0));
        }
        if (showKeyCircle) {
            currentKeySprites.Add(InstantiateSprite(keyCircle, 1));
        }
        if (showKeyTriangle) {
            currentKeySprites.Add(InstantiateSprite(keyTriangle, 2));
        }
    }

    private GameObject InstantiateSprite(Sprite sprite, int position) {
        GameObject newSpriteGo = Utils.InstantiateSpriteGameObject(sprite, Constants.SORTING_LAYER_GUI, 1, this.transform);
        float positionX = offsetPixelsX / Constants.PPU;
        positionX += position * (distancePixelsX / Constants.PPU);
        positionX += position * widthSprite;
        newSpriteGo.transform.localPosition = new Vector3(positionX, offsetPixelsY / Constants.PPU, 0);
        return newSpriteGo;
    }
}
