using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Door: AbstactInteractable {

    public Sprite closedDoor;
    public Sprite openDoor;
    public Door otherDoor;

    [HideInInspector]
    public bool inAnimation = false;
    private SpriteRenderer spriteRenderer;

    public override void Start() {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OpenDoor() {
        spriteRenderer.sprite = openDoor;
    }


    public override void Activate() {
        OpenDoor();
        actionFinished = false;
        if (!inAnimation) {
            inAnimation = true;
            StartCoroutine(FadeOut());
        }
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
        FadeCanvasEffect fe = FadeCanvasEffect.GetInstance();
        fe.FadeInCanvas();

        otherDoor.OpenDoor();
        cf.Init();

        yield return new WaitUntil(() => fe.fadeComplete);
        cf.verticalSmoothTime = verticalSmoothTimeOrigin;
        cf.Init();
        inAnimation = false;
    }
}
