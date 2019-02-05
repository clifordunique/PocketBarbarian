using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItemSpriteStatic : MonoBehaviour, IMenueItemSprite {

    
    public Sprite enabledSprite;
    private Sprite disabledSprite;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        disabledSprite = sr.sprite;
    }
    

    public void Click() {
    }

    public float GetWidth() {
        return Utils.GetWidthFromSpriteGO(gameObject);
    }

    public void SetDisabled() {
        sr.sprite = disabledSprite;
    }

    public void SetEnabled() {
        sr.sprite = enabledSprite;
    }

}
