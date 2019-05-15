using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : AbstractCutSceneController {

    public AnimationController doorClip;
    public AnimationController barbarianClip;
    public AnimationController goblinClip;
    public SimpleMovement goblinMovement;
    public PlayerController player;

    
    public float startTime = 0F;

    [Header("Door ControllTime")]
    public float doorClip1Duration = 0F;
    public float doorClip2Time = 0F;
    public float doorClip3Time = 0F;
    public float doorClip4Time = 0F;

    [Header("Goblin ControllTime")]
    public float goblinStartTalking1Time = 0F;
    public float goblinStopTalking1Time = 0F;

    public float goblinStartTalking2Time = 0F;
    public float goblinStopTalking2Time = 0F;

    public float goblinStartTalking3Time = 0F;
    public float goblinStopTalking3Time = 0F;

    public float goblinStartTalking4Time = 0F;
    public float goblinStopTalking4Time = 0F;

    public float goblinFallTime = 0F;

    [Header ("Barbarian ControllTime")]
    public float barbarianStopTalking1Time = 0F;

    public float barbarianStartTalking2Time = 0F;
    public float barbarianStopTalking2Time = 0F;

    public float barbarianStartTalking3Time = 0F;
    public float barbarianStopTalking3Time = 0F;

    public float barbarianStartTalking4Time = 0F;
    public float barbarianStopTalking4Time = 0F;

    public float barbarianStartPlayer = 0F;

    private int doorClipStep = 0;
    private int goblinClipStep = 0;
    private int barbarianClipStep = 0;

    private Dialogue dialogue;

    // Use this for initialization
    void Start () {
        dialogue = GetComponent<Dialogue>();
        barbarianClip.gameObject.SetActive(false);
        doorClip.TriggerClip("WAIT");
        player.SetEnabled(false, false);
        playCutScene = true;
    }

    // called first
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        startTime += Time.timeSinceLevelLoad;        
    }

    // Update is called once per frame
    void Update () {
        if (playCutScene) {
            // controll door
            if (doorClipStep == 0) {
                doorClip.TriggerClip("CLIP1");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > (startTime + doorClip1Duration) && doorClipStep == 1) {
                doorClip.TriggerClip("WAIT");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + doorClip2Time && doorClipStep == 2) {
                doorClip.TriggerClip("CLIP2");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + doorClip2Time && doorClipStep == 3 && doorClip.animationComplete) {
                doorClip.TriggerClip("WAIT");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + doorClip3Time && doorClipStep == 4) {
                doorClip.TriggerClip("CLIP3");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + doorClip3Time && doorClipStep == 5 && doorClip.animationComplete) {
                doorClip.TriggerClip("WAIT");
                doorClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + doorClip4Time && doorClipStep == 6) {
                doorClip.TriggerClip("CLIP4");
                doorClipStep++;
                CameraFollow.GetInstance().ShakeSmall();
            }
            if (doorClipStep == 7 && doorClip.animationTriggerReached) {
                CameraFollow.GetInstance().ShakeMedium();
                doorClipStep++;
            }


            // controll goblin
            if (Time.timeSinceLevelLoad > startTime && goblinClipStep == 0) {
                goblinClip.TriggerClip("RUN");
                goblinMovement.StartMoving();
                goblinClipStep++;
            }
            if (goblinClipStep == 1 && !goblinMovement.isMoving) {
                goblinClip.TriggerClip("IDLE");
                goblinClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + goblinStartTalking1Time && goblinClipStep == 2) {
                goblinClip.TriggerClip("TALK");
                dialogue.StartDialogue();
                goblinClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + goblinStopTalking1Time && goblinClipStep == 3) {
                goblinClip.TriggerClip("IDLE");
                dialogue.DisableLastTextBox();
                goblinClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + goblinStartTalking2Time && goblinClipStep == 4) {
                goblinClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                goblinClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + goblinStopTalking2Time && goblinClipStep == 5) {
                goblinClip.TriggerClip("IDLE");
                dialogue.DisableLastTextBox();
                goblinClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + goblinStartTalking3Time && goblinClipStep == 6) {
                goblinClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                goblinClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + goblinStopTalking3Time && goblinClipStep == 7) {
                goblinClip.TriggerClip("IDLE");
                dialogue.DisableLastTextBox();
                goblinClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + goblinStartTalking4Time && goblinClipStep == 8) {
                goblinClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                goblinClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + goblinStopTalking4Time && goblinClipStep == 9) {
                goblinClip.TriggerClip("IDLE");
                dialogue.DisableLastTextBox();
                goblinClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + goblinFallTime && goblinClipStep == 10) {
                goblinClip.TriggerClip("FALL");
                goblinClipStep++;
            }


            // controll barbarian
            if (doorClipStep == 8 && doorClip.animationComplete && barbarianClipStep == 0) {
                doorClip.TriggerClip("CLIP5");
                barbarianClip.gameObject.SetActive(true);
                barbarianClipStep++;
            }
            if (barbarianClipStep == 1 && barbarianClip.animationComplete) {
                barbarianClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                barbarianClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + barbarianStopTalking1Time && barbarianClipStep == 2) {
                barbarianClip.TriggerClip("LOOK");
                dialogue.DisableLastTextBox();
                barbarianClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + barbarianStartTalking2Time && barbarianClipStep == 3) {
                barbarianClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                barbarianClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + barbarianStopTalking2Time && barbarianClipStep == 4) {
                barbarianClip.TriggerClip("LOOK");
                dialogue.DisableLastTextBox();
                barbarianClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + barbarianStartTalking3Time && barbarianClipStep == 5) {
                barbarianClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                barbarianClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + barbarianStopTalking3Time && barbarianClipStep == 6) {
                barbarianClip.TriggerClip("LOOK");
                dialogue.DisableLastTextBox();
                barbarianClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + barbarianStartTalking4Time && barbarianClipStep == 7) {
                barbarianClip.TriggerClip("TALK");
                dialogue.NextDialogueStep();
                barbarianClipStep++;
            }
            if (Time.timeSinceLevelLoad > startTime + barbarianStopTalking4Time && barbarianClipStep == 8) {
                barbarianClip.TriggerClip("LOOK");
                dialogue.DisableLastTextBox();
                barbarianClipStep++;
            }

            if (Time.timeSinceLevelLoad > startTime + barbarianStartPlayer && barbarianClipStep == 9) {
                PrepareStartGame();
                barbarianClipStep++;
            }
        }
    }

    private void PrepareStartGame() {
        finished = true;
        barbarianClip.gameObject.SetActive(false);
        goblinClip.gameObject.SetActive(false);
        player.SetEnabled(true, true);
        doorClip.GetComponent<EdgeCollider2D>().enabled = false;
        playCutScene = false;
    }

    public override void SkipScene() {
        doorClip.TriggerClip("CLIP5");
        PrepareStartGame();
    }
}
