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
    public SkillTextBox newStatsTextbox;

    public TriggerManager triggerManagerActivateOnStart;
    public TriggerManager triggerManagerActivateOnFight;
    public TriggerManager triggerManagerActivateOnEnd;

    private bool inFight = false;
    private bool fightStarted = false;
    private bool fightFinished = false;
    private bool showNewStats = false;
    private bool everythingFinished = false;

    public override void StartDialogue() {
        base.StartDialogue();
        triggerManagerActivateOnStart.ActivateReactors();
    }

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
                    newStatsTextbox.HideTextBox();
                    InputController.GetInstance().moveInputEnabled = true;
                    StartCoroutine(CameraFollow.GetInstance().MoveCameraBack(1F, transform.position));

                    triggerManagerActivateOnStart.DeactivateReactors();
                    triggerManagerActivateOnEnd.ActivateReactors();
                    everythingFinished = true;
                }
            }
        }
    }

    IEnumerator BossKilledCoroutine() {
        GuiController.GetInstance().CanvasFlash();
        yield return new WaitForSeconds(waitTimePommesgabelAfterBossDead);
        InputController.GetInstance().moveInputEnabled = false;
        GuiController.GetInstance().ShowFantasyText("Berserk!", showNewTitleTime);
        PlayerController.GetInstance().InterruptState(AbstractState.ACTION.POMMESGABEL);
        triggerManagerActivateOnFight.DeactivateReactors();
        StartCoroutine(ShowNewStatsCoroutine(showNewTitleTime + 1));
    }


    IEnumerator ShowNewStatsCoroutine(float showInSeconds) {
        yield return new WaitForSeconds(showInSeconds);
        GuiController.GetInstance().CanvasFlash();
        newStatsTextbox.ShowTextBox();
        showNewStats = true;
    }


    IEnumerator StartEffect() {
        yield return new WaitForSeconds(delayTextEffect);
        GuiController.GetInstance().ShowRandomFightText(durationTextEffect);
        triggerManagerActivateOnFight.ActivateReactors();
        StartCoroutine(EnableControlls());
    }

    IEnumerator EnableControlls() {
        yield return new WaitForSeconds(durationTextEffect);

        InputController.GetInstance().moveInputEnabled = true;
        ITriggerReactor tm = boss.GetComponent<ITriggerReactor>();
        tm.TriggerActivated();
        fightStarted = true;
    }

}
