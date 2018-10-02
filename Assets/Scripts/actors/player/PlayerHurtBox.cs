using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : HurtBox {

    public LayerMask attackLayersUnDashable;
    public float switchBackDelayTime = 0.1F;

    private LayerMask attackLayersOrigin;
    private bool dashLayerActive = false;

    public void SwitchToDashLayer() {
        if (!dashLayerActive) {
            attackLayersOrigin = attackLayers;
            attackLayers = attackLayersUnDashable;
            dashLayerActive = true;
        }
    }

    public void SwitchToOriginLayer() {
        Invoke("SwitchBack", switchBackDelayTime);
    }

    public void SwitchBack() {
        if (dashLayerActive) {
            attackLayers = attackLayersOrigin;
            dashLayerActive = false;
        }
    }
}
