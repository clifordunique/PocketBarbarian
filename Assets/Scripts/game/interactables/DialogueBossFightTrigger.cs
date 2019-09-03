using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue))]
public class DialogueBossFightTrigger: DialogueTrigger {

    public float delayTextEffect = 1F;
    public float durationTextEffect = 1F;
    public GameObject boss;
    public float waitTimePommesgabelAfterBossDead;
    public float showNewTitleTime = 3F;
    public TextBox newStatsTextbox;
        
    private bool inFight = false;
    private bool fightStarted = false;
    private bool fightFinished = false;
    private bool showNewStats = false;
    private bool everythingFinished = false;

    public override void Update() {
        if (!everythingFinished) {
            if (!fightStarted && !dialogueFinished && inDialogue) {
                // check if Dialogue finished
                if (!dialogue.inDialogue && !inFight) {
                    inFight = true;
                    StartCoroutine(StartEffect());
                    dialogueFinished = true;
                }
            }
            if (fightStarted && !fightFinished) {
                // check if boss is still alive
                if (!boss) {
                    StartCoroutine(BossKilledCoroutine());
                    fightFinished = true;
                }
            }

            if (fightFinished && showNewStats) {
                if (Input.anyKeyDown) {
                    PlayerController.GetInstance().InterruptState(AbstractState.ACTION.SWORD_UP);
                    newStatsTextbox.gameObject.SetActive(false);
                    InputController.GetInstance().moveInputEnabled = true;
                    StartCoroutine(CameraFollow.GetInstance().MoveCameraBack(1F, transform.position));
                    everythingFinished = true;
                }
            }
        }
    }

    IEnumerator BossKilledCoroutine() {
        GuiController.GetInstance().CanvasFlash();
        yield return new WaitForSeconds(waitTimePommesgabelAfterBossDead);
        InputController.GetInstance().moveInputEnabled = false;
        GuiController.GetInstance().ShowFantasyText("Brutal!", showNewTitleTime);
        PlayerController.GetInstance().InterruptState(AbstractState.ACTION.POMMESGABEL);
        StartCoroutine(ShowNewStatsCoroutine(showNewTitleTime + 1));
    }


    IEnumerator ShowNewStatsCoroutine(float showInSeconds) {
        yield return new WaitForSeconds(showInSeconds);
        GuiController.GetInstance().CanvasFlash();
        newStatsTextbox.gameObject.SetActive(true);
        newStatsTextbox.ShowTextBox("Hello World");
        showNewStats = true;
    }


    IEnumerator StartEffect() {
        yield return new WaitForSeconds(delayTextEffect);
        GuiController.GetInstance().ShowRandomFightText(durationTextEffect);
        StartCoroutine(EnableControlls());
    }

    IEnumerator EnableControlls() {
        yield return new WaitForSeconds(durationTextEffect);

        InputController.GetInstance().moveInputEnabled = true;
        ITriggerReactor tm = boss.GetComponent<ITriggerReactor>();
        tm.TriggerActivated();
        fightStarted = true;
    }



    IEnumerator MoveCameraBack2() {
        float t = 0.0f;
        while (t <= 1.0) {
            t += Time.deltaTime / cameraMoveSeconds;
            float v = t;
            v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            Vector3 newPos = new Vector3(pixelPerfectMoveAmount.x, pixelPerfectMoveAmount.y, Camera.main.transform.position.z);
            Camera.main.transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
        CameraFollow.GetInstance().enabled = true;
        CameraFollow.GetInstance().Init();
    }
}
