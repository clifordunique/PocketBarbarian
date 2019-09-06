using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarcryStat: AbstractState {

    private bool startCry = false;
    private bool cryInProgress = false;

    private float CRY_DURATION = 1.5F;
    private float cryStartTime = 0F;

    private float raaaEffectOffsetX = 1F;
    private float raaaEffectOffsetY = 1.5F;

    private bool animEnd = false;

    public WarcryStat(PlayerController playerController) : base(ACTION.WARCRY, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(WARCRY, true);
        Move(0, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(WARCRY, false);
        playerController.animator.SetBool(WARCRY_OUT, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }     
        
       
        if (startCry) {
            cryStartTime = Time.timeSinceLevelLoad;
            CameraFollow.GetInstance().ShakeCamera(CRY_DURATION, 0.1F, 0.1F);
            playerController.InstantiateEffect(playerController.prefabEffectRaaa, raaaEffectOffsetX, raaaEffectOffsetY, false);
            playerController.InstantiateEffect(playerController.prefabEffectWarcrySoundWave);
            Debug.Log("Start crying");
            startCry = false;
        }

        if (cryInProgress && cryStartTime + CRY_DURATION < Time.timeSinceLevelLoad) {
            // cry finished
            playerController.animator.SetBool(WARCRY, false);
            playerController.animator.SetBool(WARCRY_OUT, true);            
        }

        if (cryInProgress && animEnd) {
            return new IdleState(playerController);
        }
        
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_START && !cryInProgress) {
            startCry = true;
            cryInProgress = true;
        }

        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animEnd = true;
        }
    }
}
