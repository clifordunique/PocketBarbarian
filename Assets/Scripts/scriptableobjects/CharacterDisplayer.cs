﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Character {
    public Sprite charSprite;
    public int widthInPixel;
}

[System.Serializable]
public struct Range {
    public int from;
    public int to;
}

[CreateAssetMenu]
public class CharacterDisplayer : ScriptableObject {

    
    public int defaultCharacterWidthUppercase;
    public int defaultCharacterWidthLowercase;
    public int defaultCharacterWidthSpecial;
    public Character[] characterUppercaseList;
    public Character[] characterLowercaseList;
    public Character[] characterSpecialList;
    
    public Range asciiRangeUppercase;
    public Range asciiRangeLowercase;
    public Range asciiRangeSpecial;
    public Range asciiRangeBlank;
    public int charSpacingPixel;

    public void OnEnable() {
        for(int i = 0; i < characterUppercaseList.Length; i++) {
            if (characterUppercaseList[i].widthInPixel == 0) {
                characterUppercaseList[i].widthInPixel = defaultCharacterWidthUppercase;
            }
        }
        for (int i = 0; i < characterLowercaseList.Length; i++) {
            if (characterLowercaseList[i].widthInPixel == 0) {
                characterLowercaseList[i].widthInPixel = defaultCharacterWidthLowercase;
            }
        }
        for (int i = 0; i < characterSpecialList.Length; i++) {
            if (characterSpecialList[i].widthInPixel == 0) {
                characterSpecialList[i].widthInPixel = defaultCharacterWidthSpecial;
            }
        }
    }

    public float DisplayString(string text, Transform myTransform, int offsetPixelsX, int offsetPixelsY) {
        if (text != null) {
            Vector2 position = new Vector2((myTransform.position.x + offsetPixelsX / Constants.PPU), (myTransform.position.y + offsetPixelsY / Constants.PPU));
            Vector2 originalPosition = position;
            for(int i = 0; i < text.Length; i++ ) {
                Debug.Log(text[i] +"="+ ((int)text[i]));
                DisplayCharacter(text[i], myTransform, ref position);
            }
            Debug.Log("Length: " + (position.x - originalPosition.x));
            return (position.x - originalPosition.x);
        }
        return -1;
    }

    private void DisplayCharacter(char character, Transform myTransform, ref Vector2 position) {
        int ascii = (int)character;
        if (ascii >= asciiRangeUppercase.from && ascii <= asciiRangeUppercase.to) {
            // Uppercase Character
            DisplayCharacterFromArray((ascii - asciiRangeUppercase.from), characterUppercaseList, myTransform, ref position);
        } else {
            if (ascii >= asciiRangeLowercase.from && ascii <= asciiRangeLowercase.to) {
                // Lowercase Character
                DisplayCharacterFromArray((ascii - asciiRangeLowercase.from), characterLowercaseList, myTransform, ref position);
            } else {
                // Blank Characters
                if (ascii >= asciiRangeBlank.from && ascii <= asciiRangeBlank.to) {
                    position.x += (defaultCharacterWidthSpecial) / Constants.PPU;
                }
                // Special Characters
                if (ascii >= asciiRangeSpecial.from && ascii <= asciiRangeSpecial.to) {
                    DisplaySpecialCharacterFromArray(ascii, myTransform, ref position);
                }
            }
        }

    }

    private void DisplaySpecialCharacterFromArray(int ascii, Transform myTransform, ref Vector2 position) {
        if (ascii == 33) {
            // !
            CreateNewChar(characterSpecialList[0], myTransform, ref position);
        }
        if (ascii == 58) {
            // :
            CreateNewChar(characterSpecialList[1], myTransform, ref position);
        }
        if (ascii == 46) {
            // .
            CreateNewChar(characterSpecialList[2], myTransform, ref position);
        }
        if (ascii == 44) {
            // ,
            CreateNewChar(characterSpecialList[3], myTransform, ref position);
        }
        if (ascii == 63) {
            // ?
            CreateNewChar(characterSpecialList[4], myTransform, ref position);
        }
        if (ascii == 45) {
            // -
            CreateNewChar(characterSpecialList[5], myTransform, ref position);
        }
        if (ascii == 43) {
            // +
            CreateNewChar(characterSpecialList[6], myTransform, ref position);
        }
    }


    private void DisplayCharacterFromArray(int index, Character[] characterList, Transform myTransform, ref Vector2 position) {
        if (index >= 0 && index < characterList.Length) {
            Character character = characterList[index];
            CreateNewChar(character, myTransform, ref position);
        }
    }

    private void CreateNewChar(Character character, Transform myTransform, ref Vector2 position) {
        GameObject spriteObject = Utils.InstantiateSpriteGameObject(character.charSprite, Constants.SORTING_LAYER_DIALOGUE, 10, myTransform);
        spriteObject.transform.position = position;
        position.x += (character.widthInPixel + charSpacingPixel) / Constants.PPU;
    }
}
