using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DoorLocked : AbstactLockedInteractable {

    public DoorKeySymbols keySymbols;
    public Sprite closedDoor;
    public Sprite openDoor;    
    public DoorLocked otherDoor;
    public Animator fadeAnimator;
    public Image uiImage;
    public float waitOpenTime;
 
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


    public override void Activate() {
        actionFinished = false;
        float actualWaitOpenTime = waitOpenTime;
        if (locked) {
            if (Unlockable()) {
                // nicht durch die tuer gehen sondern erst aufschliessen
                Unlock();
                otherDoor.locked = false;
                otherDoor.keySymbol.SetActive(false);
                actualWaitOpenTime += unlockTime + waitAfterUnlockTime;
            } else {
                // locked - keine Action
                return;
            }
        }
        StartCoroutine(WalkThroughDoor(actualWaitOpenTime));
    }


    IEnumerator WalkThroughDoor(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        OpenDoor();        
        if (!inAnimation) {
            inAnimation = true;
            StartCoroutine(FadeOut());
        }
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoor;
    }


    IEnumerator FadeOut() {
        fadeAnimator.SetBool("FADE_IN", false);
        fadeAnimator.SetBool("FADE_OUT", true);
        yield return new WaitUntil(()=> uiImage.color.a == 1);
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
        inAnimation = false;
    }
}
