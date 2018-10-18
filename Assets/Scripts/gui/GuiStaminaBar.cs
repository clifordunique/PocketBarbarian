using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiStaminaBar : MonoBehaviour {

    public Sprite barElement;
    public int offsetPixelsX;
    public int offsetPixelsY;
    public float distancePixelsX = 0F;
    public float delayTimeStart = 0.1F;
    public float delayTime = 0.1F;

    public int maxBars;
    public int currentBars;
    public int newBars;

    private List<GameObject> currentBarArray = new List<GameObject>();
    private float widthSprite;    
    private bool coroutineRunning = false;

    // Use this for initialization
    void Start () {
        widthSprite = barElement.bounds.size.x;        
    }

    public void UpdateStamina(int newStaminaBars) {
        Debug.Log("Update Stamina:" + newStaminaBars);
        newBars = (newStaminaBars > maxBars ? maxBars : newStaminaBars);
        newBars = (newBars < 0 ? 0 : newBars);
        if (!coroutineRunning) {
            StartCoroutine(ChangeBar());
        }
    }
    

    IEnumerator ChangeBar() {
        coroutineRunning = true;
        Debug.Log("ChangeBar: " + newBars + " / " + currentBars);
        while (newBars != currentBars && newBars >= 0) {
            
            // delete bars
            if (newBars < currentBars) {
                if (currentBars == currentBarArray.Count) {
                    GameObject destroyObject = currentBarArray[currentBarArray.Count - 1];
                    currentBarArray.Remove(destroyObject);
                    Destroy(destroyObject);
                    currentBars--;
                } else {
                    Debug.LogError("GuiStamina: currentBars <> currentBarArray!");
                }
            } else {
                // add new bars
                
                GameObject newBar = Utils.InstantiateSpriteGameObject(barElement, Constants.SORTING_LAYER_GUI, 1, this.transform);

                float positionX = offsetPixelsX / Constants.PPU;
                positionX += currentBars * (distancePixelsX / Constants.PPU);
                positionX += currentBars * widthSprite;
                newBar.transform.localPosition = new Vector3(positionX, offsetPixelsY / Constants.PPU, 0);
                currentBarArray.Add(newBar);
                currentBars++;
            }
            yield return new WaitForSeconds(delayTime);
        }
        coroutineRunning = false;
    }
}
