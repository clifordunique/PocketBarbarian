using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPoolManager : MonoBehaviour
{
    public int poolSize;
    public int additionalPoolsize;
    public Sprite[] spriteList;

    private Dictionary<Sprite, CharacterPool> characterPools = new Dictionary<Sprite, CharacterPool>();

    private static CharacterPoolManager _instance;

    private void Awake() {
        _instance = this;
    }

    public static CharacterPoolManager GetInstance() {
        if (_instance == null) {
            Debug.LogError("CharacterPoolManager not yet instanciated!");
            return null;
        } else {
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < spriteList.Length; i++) {
            characterPools.Add(spriteList[i], new CharacterPool(spriteList[i], poolSize, additionalPoolsize, transform));            
        }
    }


    public GameObject GetCharacter(Sprite charSprite) {
        return characterPools[charSprite].GetCharacter();
    }

    public void FreeCharacter(GameObject charObject) {
        SpriteRenderer renderer = charObject.GetComponent<SpriteRenderer>();
        if (renderer) {
            characterPools[renderer.sprite].FreeCharacter(charObject);
        }
    }
   
}
