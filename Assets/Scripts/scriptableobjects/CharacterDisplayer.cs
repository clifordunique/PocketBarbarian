using System.Collections;
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
    public int defaultCharacterWidthNumbers;
    public int defaultCharacterWidthSpecial;
    public Character[] characterUppercaseList;
    public Character[] characterLowercaseList;
    public Character[] characterNumbersList;
    public Character[] characterSpecialList;
    
    public Range asciiRangeUppercase;
    public Range asciiRangeLowercase;
    public Range asciiRangeNumbers;
    public Range asciiRangeSpecial;
    public Range asciiRangeBlank;
    public int charSpacingPixel;
    public int charPixelHeight;


    public bool charsUseSpriteMask = false;

    private static int LINEBREAK = 92;

    private float maxLineWidth = float.MinValue;
    

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
        for (int i = 0; i < characterNumbersList.Length; i++) {
            if (characterNumbersList[i].widthInPixel == 0) {
                characterNumbersList[i].widthInPixel = defaultCharacterWidthNumbers;
            }
        }
        for (int i = 0; i < characterSpecialList.Length; i++) {
            if (characterSpecialList[i].widthInPixel == 0) {
                characterSpecialList[i].widthInPixel = defaultCharacterWidthSpecial;
            }
        }
    }

    public Vector2 DisplayString(string text, Transform myTransform, int offsetPixelsX, int offsetPixelsY, int additionalLineSpacePixel = 0, bool alignCenter = false) {
        if (text != null) {
            Vector2 position = new Vector2((myTransform.position.x + offsetPixelsX / Constants.PPU), (myTransform.position.y + offsetPixelsY / Constants.PPU));
            Vector2 originalPosition = position;
            maxLineWidth = float.MinValue;

            List<GameObject> lines = new List<GameObject>();
            int linePos = AddNewLine(ref lines, myTransform);

            for (int i = 0; i < text.Length; i++ ) {
                
                if (position.x > maxLineWidth) {
                    // laengste Zeile merken
                    maxLineWidth = position.x;
                }
                
                if (((int)text[i]) == LINEBREAK || text[i] == '\n') {

                    if (alignCenter) {
                        // center line
                        CenterLine(position.x, originalPosition.x, lines[linePos].transform);
                    }

                    position.x = originalPosition.x;
                    position.y -= (charPixelHeight + charSpacingPixel + additionalLineSpacePixel) / Constants.PPU;
                    linePos = AddNewLine(ref lines, myTransform);

                } else {
                    DisplayCharacter(text[i], lines[linePos].transform, ref position);
                }
            }
            if (alignCenter) {
                // center last line
                CenterLine(position.x, originalPosition.x, lines[linePos].transform);
            }

            return new Vector2((maxLineWidth - originalPosition.x), (originalPosition.y - position.y));
        }
        return Vector2.zero;
    }

    private int AddNewLine(ref List<GameObject> lines, Transform myTransform) {
        lines.Add(new GameObject());
        int position = lines.Count - 1;
        lines[position].transform.parent = myTransform;
        lines[position].transform.localPosition = Vector3.zero;
        return position;
    }

    private void CenterLine(float currentX, float originalX, Transform line) {
        // center line
        float correction = (currentX - originalX) / 2;
        line.position = Utils.MakePixelPerfect(line.position + Vector3.left * correction);
        //line.position = line.position + Vector3.left * correction;
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
                if (ascii >= asciiRangeNumbers.from && ascii <= asciiRangeNumbers.to) {
                    // Number Character
                    DisplayCharacterFromArray((ascii - asciiRangeNumbers.from), characterNumbersList, myTransform, ref position);
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
        if (ascii == 42) {
            // *
            CreateNewChar(characterSpecialList[7], myTransform, ref position);
        }
        if (ascii == 39) {
            // '
            CreateNewChar(characterSpecialList[8], myTransform, ref position);
        }
        if (ascii == 37) {
            // % --> Line-effect start/end
            CreateNewChar(characterSpecialList[9], myTransform, ref position);
        }
        if (ascii == 95) {
            // _ --> Line-effect 
            CreateNewChar(characterSpecialList[10], myTransform, ref position);
        }
        if (ascii == 35) {
            // # --> Return symbol 
            CreateNewChar(characterSpecialList[11], myTransform, ref position);
        }
    }


    private void DisplayCharacterFromArray(int index, Character[] characterList, Transform myTransform, ref Vector2 position) {
        if (index >= 0 && index < characterList.Length) {
            Character character = characterList[index];
            CreateNewChar(character, myTransform, ref position);
        }
    }

    private void CreateNewChar(Character character, Transform myTransform, ref Vector2 position) {
        GameObject spriteObject = Utils.InstantiateSpriteGameObject(character.charSprite, Constants.SORTING_LAYER_DIALOGUE, 10, myTransform, charsUseSpriteMask);
        spriteObject.transform.position = position;
        position.x += (character.widthInPixel + charSpacingPixel) / Constants.PPU;
    }
}
