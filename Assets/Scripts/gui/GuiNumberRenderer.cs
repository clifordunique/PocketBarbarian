using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiNumberRenderer : MonoBehaviour {

    public NumberDisplayer numbers;
    public float offsetPixelsX = 0F;
    public float offsetPixelsY = 0F;

    public float delayTime = 0.1F;

    private int currentNumber = 0;

    // Use this for initialization
    void Start () {
        Invoke("UpdateNumber", delayTime);
    }

    public void UpdatePotion(CollectablePotions.POTION_TYPE potionType, int number) {
        currentNumber = number;
        Invoke("UpdateNumber", delayTime);
    }

    private void UpdateNumber() {
        numbers.UpdateNumber(currentNumber, this.transform, offsetPixelsX, offsetPixelsY);
    }
	
}
