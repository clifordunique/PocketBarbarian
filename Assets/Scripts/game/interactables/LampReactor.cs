using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampReactor : MonoBehaviour, ITriggerReactor {

    public Sprite lampOnSprite;
    public Sprite lampOffSprite;
    public bool defaultOn = true;
    public bool onActiveLightsOn = true;

    private bool lightOn = false;
    
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (defaultOn) {
            LightOn();
        } else {
            LightOff();
        }
    }

    private void LightOn() {
        lightOn = true;
        sr.sprite = lampOnSprite;
    }

    private void LightOff() {
        lightOn = false;
        sr.sprite = lampOffSprite;
    }

    public bool TriggerActivated() {
        if (onActiveLightsOn) {
            LightOn();
        } else {
            LightOff();
        }
        return true;
    }

    public bool TriggerDeactivated() {
        if (onActiveLightsOn) {
            LightOff();
        } else {
            LightOn();
        }
        return true;
    }
}
