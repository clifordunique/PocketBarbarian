using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : HurtBox {

    public LayerMask attackLayersUnDashable;
    public float switchBackDelayTime = 0.1F;

    private LayerMask attackLayersOrigin;

    public void SwitchToDashLayer() {
        attackLayersOrigin = attackLayers;
        attackLayers = attackLayersUnDashable;
    }

    public void SwitchToOriginLayer() {
        Invoke("SwitchBack", switchBackDelayTime);
    }

    private void SwitchBack() {
        attackLayers = attackLayersOrigin;
    }
}
