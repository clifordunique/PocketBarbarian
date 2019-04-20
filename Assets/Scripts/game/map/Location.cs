using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public MapController.LOCATION location = MapController.LOCATION.NA;
    public MapController.LOCATION locationLeft = MapController.LOCATION.NA;
    public MapController.LOCATION locationRight = MapController.LOCATION.NA;
    public MapController.LOCATION locationUp = MapController.LOCATION.NA;
    public MapController.LOCATION locationDown = MapController.LOCATION.NA;

    public Sprite markedSprite;

    public GameObject pointer;

    private Sprite normalSprite;
    private SpriteRenderer spriteRenderer;
    private float interval = 0.15F;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = spriteRenderer.sprite;
    }

    public void DisablePointer() {
        pointer.SetActive(false);
    }

    public void EnablePointer() {
        pointer.SetActive(true);
    }

    public void Mark() {
        DisablePointer();
        StartCoroutine(Flashing(1.0F));
    }

    public MapController.LOCATION GetLocationLeft() {
        return locationLeft;
    }

    public MapController.LOCATION GetLocationRight() {
        return locationRight;
    }

    public MapController.LOCATION GetLocationUp() {
        return locationUp;
    }

    public MapController.LOCATION GetLocationDown() {
        return locationDown;
    }


    public IEnumerator Flashing(float time) {
        float flashCount = Mathf.Round(time / interval);
        for (int i = 0; i < flashCount; i++) {
            // switch color
            if (i % 2 == 0) {
                spriteRenderer.sprite = markedSprite;
            }
            if (i % 2 == 1) {
                spriteRenderer.sprite = normalSprite;
            }
            yield return new WaitForSeconds(interval);
        }
        spriteRenderer.sprite = markedSprite;
    }
}
