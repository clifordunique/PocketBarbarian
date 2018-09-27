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
    public GameObject actionArrow;
    public LayerMask reactLayer;

    [HideInInspector]
    public bool inAnimation = false;
    private SpriteRenderer spriteRenderer;
    [HideInInspector]
    public bool actionFinished = false;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoor;
    }

    private void CloseDoor() {
        spriteRenderer.sprite = closedDoor;
    }

    public void Activate() {
        OpenDoor();
        actionFinished = false;
        if (!inAnimation) {
            inAnimation = true;
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
        float verticalSmoothTimeOrigin = cf.verticalSmoothTime;
        cf.verticalSmoothTime = 0;
        cf.transform.position = otherDoor.transform.position;

        actionFinished = true;
        StartCoroutine(FadeIn(cf, verticalSmoothTimeOrigin));
    }

    IEnumerator FadeIn(CameraFollow cf, float verticalSmoothTimeOrigin) {
        otherDoor.OpenDoor();
        cf.Init();
        fadeAnimator.SetBool("FADE_OUT", false);
        fadeAnimator.SetBool("FADE_IN", true);
        yield return new WaitUntil(() => uiImage.color.a == 0);

        cf.verticalSmoothTime = verticalSmoothTimeOrigin;
        otherDoor.CloseDoor();
        inAnimation = false;
    }


    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            Debug.Log("IN TRIGGER ENTER DOOR!");
            actionArrow.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            Debug.Log("IN TRIGGER ENTER DOOR!");
            actionArrow.SetActive(false);
        }
    }
}
