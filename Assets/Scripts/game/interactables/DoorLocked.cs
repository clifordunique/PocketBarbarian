using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DoorLocked : AbstactLockedInteractable {

    public DoorKeySymbols keySymbols;
    public Sprite closedDoor;
    public Sprite openDoor;    
    public DoorLocked otherDoor;
    public float waitOpenTime;
 
    [HideInInspector]
    public bool inAnimation = false;
    private SpriteRenderer spriteRenderer;
 
    public override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (keySymbols) {
            keySymbols.gameObject.SetActive(true);
            keySymbols.InitKeySymbol(lockedKey);
        }
    }


    public override void Activate() {
        actionFinished = false;
        float actualWaitOpenTime = waitOpenTime;
        if (!open) {
            if (Unlockable()) {
                // nicht durch die tuer gehen sondern erst aufschliessen
                Unlock();
                otherDoor.Unlock();
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
        FadeCanvasEffect fe = FadeCanvasEffect.GetInstance();
        fe.FadeOutCanvas();
        yield return new WaitUntil(() => fe.fadeComplete);
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

        FadeCanvasEffect fe = FadeCanvasEffect.GetInstance();
        fe.FadeInCanvas();
        yield return new WaitUntil(() => fe.fadeComplete);

        cf.verticalSmoothTime = verticalSmoothTimeOrigin;
        inAnimation = false;
    }
}
