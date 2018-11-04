using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour {

    public AnimationController doorClip;
    public AnimationController barbarianClip;
    public AnimationController goblinClip;
    public SimpleMovement goblinMovement;

    
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

    private int doorClipStep = 0;
    private int goblinClipStep = 0;
    private int barbarianClipStep = 0;

    private Dialogue dialogue;

    // Use this for initialization
    void Start () {
        dialogue = GetComponent<Dialogue>();
        barbarianClip.gameObject.SetActive(false);
        doorClip.TriggerClip("WAIT");        
    }

    // called first
    void OnEnable() {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        startTime += Time.time;
    }

    // Update is called once per frame
    void Update () {

        // controll door
        if (doorClipStep == 0) {
            doorClip.TriggerClip("CLIP1");
            doorClipStep++;
        }
        if (Time.time > (startTime + doorClip1Duration) && doorClipStep == 1) {
            doorClip.TriggerClip("WAIT");
            doorClipStep++;
        }
        if (Time.time > startTime + doorClip2Time && doorClipStep == 2) {
            doorClip.TriggerClip("CLIP2");
            doorClipStep++;
        }
        if (Time.time > startTime + doorClip2Time && doorClipStep == 3 && doorClip.animationComplete) {
            doorClip.TriggerClip("WAIT");
            doorClipStep++;
        }
        if (Time.time > startTime + doorClip3Time && doorClipStep == 4) {
            doorClip.TriggerClip("CLIP3");
            doorClipStep++;
        }
        if (Time.time > startTime + doorClip3Time && doorClipStep == 5 && doorClip.animationComplete) {
            doorClip.TriggerClip("WAIT");
            doorClipStep++;
        }
        if (Time.time > startTime + doorClip4Time && doorClipStep == 6) {
            doorClip.TriggerClip("CLIP4");
            doorClipStep++;
        }


        // controll goblin
        if (Time.time > startTime && goblinClipStep == 0) {
            goblinClip.TriggerClip("RUN");
            goblinMovement.StartMoving();
            goblinClipStep++;
        }
        if (goblinClipStep == 1 && !goblinMovement.isMoving) {
            goblinClip.TriggerClip("IDLE");
            goblinClipStep++;
        }
        if (Time.time > startTime + goblinStartTalking1Time && goblinClipStep == 2) {
            goblinClip.TriggerClip("TALK");
            dialogue.StartDialogue();
            goblinClipStep++;
        }
        if (Time.time > startTime + goblinStopTalking1Time && goblinClipStep == 3) {
            goblinClip.TriggerClip("IDLE");
            dialogue.DisableLastTextBox();
            goblinClipStep++;
        }

        if (Time.time > startTime + goblinStartTalking2Time && goblinClipStep == 4) {
            goblinClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            goblinClipStep++;
        }
        if (Time.time > startTime + goblinStopTalking2Time && goblinClipStep == 5) {
            goblinClip.TriggerClip("IDLE");
            dialogue.DisableLastTextBox();
            goblinClipStep++;
        }

        if (Time.time > startTime + goblinStartTalking3Time && goblinClipStep == 6) {
            goblinClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            goblinClipStep++;
        }
        if (Time.time > startTime + goblinStopTalking3Time && goblinClipStep == 7) {
            goblinClip.TriggerClip("IDLE");
            dialogue.DisableLastTextBox();
            goblinClipStep++;
        }

        if (Time.time > startTime + goblinStartTalking4Time && goblinClipStep == 8) {
            goblinClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            goblinClipStep++;
        }
        if (Time.time > startTime + goblinStopTalking4Time && goblinClipStep == 9) {
            goblinClip.TriggerClip("IDLE");
            dialogue.DisableLastTextBox();
            goblinClipStep++;
        }

        if (Time.time > startTime + goblinFallTime && goblinClipStep == 10) {
            Debug.Log("Goblin fall");
            goblinClip.TriggerClip("FALL");
            goblinClipStep++;
        }


        // controll barbarian
        if (doorClipStep == 7 && doorClip.animationComplete && barbarianClipStep == 0) {
            barbarianClip.gameObject.SetActive(true);
            barbarianClipStep++;
        }
        if (barbarianClipStep == 1 && barbarianClip.animationComplete) {
            barbarianClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            barbarianClipStep++;
        }

        if (Time.time > startTime + barbarianStopTalking1Time && barbarianClipStep == 2) {
            barbarianClip.TriggerClip("LOOK");
            dialogue.DisableLastTextBox();
            barbarianClipStep++;
        }

        if (Time.time > startTime + barbarianStartTalking2Time && barbarianClipStep == 3) {
            barbarianClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            barbarianClipStep++;
        }
        if (Time.time > startTime + barbarianStopTalking2Time && barbarianClipStep == 4) {
            barbarianClip.TriggerClip("LOOK");
            dialogue.DisableLastTextBox();
            barbarianClipStep++;
        }

        if (Time.time > startTime + barbarianStartTalking3Time && barbarianClipStep == 5) {
            barbarianClip.TriggerClip("TALK");
            dialogue.NextDialogueStep();
            barbarianClipStep++;
        }
        if (Time.time > startTime + barbarianStopTalking3Time && barbarianClipStep == 6) {
            barbarianClip.TriggerClip("LOOK");
            dialogue.DisableLastTextBox();
            barbarianClipStep++;
        }
    }
}
