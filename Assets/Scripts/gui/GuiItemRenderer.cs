using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiItemRenderer : MonoBehaviour
{
    public List<ItemIcon> availableItems;
    public ItemIcon selectedItem;

    public float offsetPixelsX = 0F;
    public float offsetPixelsY = 0F;

    public float delayTime = 0.1F;    

    private string gameObjectName = "SPRITE";
    private GuiNumberRenderer numberRenderer;

    // Use this for initialization
    void Start() {
        numberRenderer = GetComponent<GuiNumberRenderer>();
    }
    

    public void UpdateItem(Item item) {
        // check if item is registered
        ItemIcon result = availableItems.Find(x => x.itemUuid.Equals(item.uuid));
        if (result != null) {
            // Item is registered
            selectedItem = result;
            
            // Update Number
            numberRenderer.UpdateNumber(item.amount);
            // Update Sprite
            Invoke("UpdateSprite", delayTime);
        }
    }

    public void UpdateSprite() {
        // delete old sprite
        Transform child = transform.Find(gameObjectName);
        if (child != null) {
            Destroy(child.gameObject);
        }
        if (selectedItem.sprite != null) {
            InstantiateSprite(selectedItem.sprite);
        }
    }


    private GameObject InstantiateSprite(Sprite sprite) {
        GameObject newSpriteGo = Utils.InstantiateSpriteGameObject(sprite, Constants.SORTING_LAYER_GUI, 1, this.transform);
        float positionX = offsetPixelsX / Constants.PPU;
        newSpriteGo.transform.localPosition = new Vector3(positionX, offsetPixelsY / Constants.PPU, 0);
        newSpriteGo.name = gameObjectName;
        return newSpriteGo;
    }

    [System.Serializable]
    public class ItemIcon {
        public string itemUuid;
        public Sprite sprite;
    }
}
