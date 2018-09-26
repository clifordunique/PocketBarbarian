using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door : MonoBehaviour {

    public Sprite closedDoor;
    public Sprite openDoor;    
    public Door otherDoor;
    public Animator fadeAnimator;
    public Image uiImage;

    private bool inEnterDoor = false;
    private SpriteRenderer spriteRenderer;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor() {
        spriteRenderer.sprite = openDoor;
    }

    public void CloseDoor() {
        spriteRenderer.sprite = closedDoor;
    }

    public void EnterDoor() {
        if (!inEnterDoor) {
            inEnterDoor = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut() {
        fadeAnimator.SetBool("FADE_IN", false);
        fadeAnimator.SetBool("FADE_OUT", true);
        yield return new WaitUntil(()=> uiImage.color.a == 1);
        CloseDoor();
        Teleport();
    }

    private void Teleport() {
        PlayerController player = PlayerController.GetInstance();
        player.transform.position = otherDoor.transform.position;
        CameraFollow cf = CameraFollow.GetInstance();
        Vector2 focusAreaSizeOrigin = cf.focusAreaSize;
        cf.focusAreaSize = new Vector2(1, 1);
        cf.Init();
        cf.transform.position = otherDoor.transform.position;

        StartCoroutine(FadeIn(cf, focusAreaSizeOrigin));
    }

    IEnumerator FadeIn(CameraFollow cf, Vector2 focusAreaSizeOrigin) {
        otherDoor.OpenDoor();
        cf.focusAreaSize = focusAreaSizeOrigin;
        cf.Init();
        fadeAnimator.SetBool("FADE_OUT", false);
        fadeAnimator.SetBool("FADE_IN", true);
        yield return new WaitUntil(() => uiImage.color.a == 0);
        otherDoor.CloseDoor();
        inEnterDoor = false;
    }
}
