using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool
{
    private List<GameObject> availableCharacters = new List<GameObject>();
    private List<GameObject> usedCharacters = new List<GameObject>();
    private Sprite characterSprite;
    private int additionalPoolsize;
    private Transform defaultParent;

    public CharacterPool(Sprite characterSprite, int initialPoolsize, int additionalPoolsize, Transform defaultParent) {
        this.characterSprite = characterSprite;
        this.additionalPoolsize = additionalPoolsize;
        this.defaultParent = defaultParent;

        CreateNewFreeCharacterObjects(initialPoolsize);
    }

    private void CreateNewFreeCharacterObjects(int newCharacterCount) {
        for (int i = 0; i < newCharacterCount; i++) {
            GameObject characterSpriteObject = Utils.InstantiateSpriteGameObject(characterSprite, Constants.SORTING_LAYER_DIALOGUE, 10, defaultParent);
            characterSpriteObject.SetActive(false);
            availableCharacters.Add(characterSpriteObject);
        }
    }

    public GameObject GetCharacter() {

        if (availableCharacters.Count <= 0) {
            CreateNewFreeCharacterObjects(additionalPoolsize);
        }
        GameObject freeCharacter = availableCharacters[0];
        freeCharacter.SetActive(true);
        availableCharacters.Remove(freeCharacter);        
        usedCharacters.Add(freeCharacter);

        return freeCharacter;
    }


    public void FreeCharacter(GameObject freeCharacter) {
        if (usedCharacters.Contains(freeCharacter)) {
            freeCharacter.transform.parent = defaultParent;
            freeCharacter.SetActive(false);
            availableCharacters.Add(freeCharacter);
            usedCharacters.Remove(freeCharacter);
        }
    }

}
