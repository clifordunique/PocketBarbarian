using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NumberDisplayer: ScriptableObject {

    public Sprite[] numbers;
    
    private float spriteWidth = -1;

    public void UpdateNumber(int newNumber, Transform parent, float offsetPixelsX, float offsetPixelsY) {
        if (spriteWidth < 0) {
            InitSpriteWidth();
        }
        ClearOldNumbers(parent);
        CreateNewNumbers(newNumber, parent, offsetPixelsX, offsetPixelsY);
    }

    private void CreateNewNumbers(int newNumber, Transform parent, float offsetPixelsX, float offsetPixelsY) {
        string numberString = newNumber.ToString();
        for (int i = 0; i < numberString.Length; i++) {
            int position = numberString.Length - 1 - i;
            int num = (int)char.GetNumericValue(numberString[position]);
            Sprite numSprite = numbers[num];
            GameObject newGameObject = CreateNewNumber(numSprite, parent, i, offsetPixelsX, offsetPixelsY);            
        }
    }

    private GameObject CreateNewNumber(Sprite sprite, Transform parent, int position, float offsetPixelsX, float offsetPixelsY) {
        GameObject newSpriteGo = Utils.InstantiateSpriteGameObject(sprite, Constants.SORTING_LAYER_GUI, 1, parent);
        float positionX = offsetPixelsX / Constants.PPU;
        positionX -= position * spriteWidth;
        newSpriteGo.transform.localPosition = new Vector3(positionX, offsetPixelsY / Constants.PPU, 0);
        return newSpriteGo;
    }

    private void InitSpriteWidth() {
        // try to find out how wide the sprites are (all sprites have to be equally wide
        if (numbers.Length > 0) {
            spriteWidth = numbers[0].bounds.size.x;
        }
    }

    private void ClearOldNumbers(Transform parent) {
        // delete old list of numbers
        foreach (Transform child in parent) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
