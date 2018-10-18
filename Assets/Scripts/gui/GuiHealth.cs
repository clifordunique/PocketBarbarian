using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiHealth : MonoBehaviour {

    public Sprite prefabHeartFull;
    public Sprite prefabHeartEmpty;

    public float offsetPixelsX = 0F;
    public float offsetPixelsY = 0F;
    public float distancePixelsX = 0F;

    public float delayTime = 0.1F;
    private float widthSprite;
    private List<GameObject> currentHeartObjectArray = new List<GameObject>();

    private int showFullHearts;
    private int showEmptyHearts;

    // Use this for initialization
    void Start () {
        widthSprite = prefabHeartFull.bounds.size.x;
    }

    public void UpdateHealth(int maxHealth, int currentHealth) {
        showFullHearts = currentHealth;
        showEmptyHearts = maxHealth - currentHealth;
        Invoke("UpdateHealth", delayTime);
    }

    public void UpdateHealth() {
        // delete old list of hearts
        foreach (GameObject heart in currentHeartObjectArray) {
            Destroy(heart);
        }
        currentHeartObjectArray.Clear();

        for (int i = 0; i< showFullHearts; i++) {
            currentHeartObjectArray.Add(InstantiateSprite(prefabHeartFull, i));
        }

        if (showEmptyHearts > 0) {
            for (int i = 0; i < showEmptyHearts; i++) {
                currentHeartObjectArray.Add(InstantiateSprite(prefabHeartEmpty, showFullHearts + i));
            }
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
