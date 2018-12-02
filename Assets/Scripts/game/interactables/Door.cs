using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door : AbstactInteractable {

    public DoorKeySymbols keySymbols;
    public Sprite closedDoor;
    public Sprite openDoor;    
    public Door otherDoor;
    public Animator fadeAnimator;
    public Image uiImage;
 
    [HideInInspector]
    public bool inAnimation = false;
    private SpriteRenderer spriteRenderer;
 
    public override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (locked && keySymbols) {
            keySymbols.gameObject.SetActive(true);
            keySymbols.InitKeySymbol(lockedKey);
        }
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoor;
    }

    private void CloseDoor() {
        spriteRenderer.sprite = closedDoor;
    }

    public override void Activate() {
        if (locked && Unlockable()) {
            // nicht durch die tuer gehen sondern aufschliessen
            Unlock();
            otherDoor.locked = false;
            otherDoor.keySymbol.SetActive(false);
        } else {
            OpenDoor();
            actionFinished = false;
            if (!inAnimation) {
                inAnimation = true;
                StartCoroutine(FadeOut());
            }
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
}
