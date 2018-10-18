using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntIntEvent: UnityEvent<int, int> { }

public class PlayerHurtBox : HurtBox {


    public LayerMask attackLayersUnDashable;
    public float switchBackDelayTime = 0.1F;

    private LayerMask attackLayersOrigin;
    private bool dashLayerActive = false;

    private static PlayerHurtBox _instance;
    [SerializeField]
    private IntIntEvent someEvent;

    public void Awake() {
        _instance = this;    
    }

    public static PlayerHurtBox GetInstance() {
        return _instance;
    }

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
            // switch boxCollider off and on to receive a new collision if still in a enemy collider
            boxCollider.enabled = false;
            Invoke("EnableBoxCollider", 0.01F);
        }
    }

    public void EnableBoxCollider() {
        boxCollider.enabled = true;
    }

    public override void ModifyHealth(int healthModifier) {
        base.ModifyHealth(healthModifier);
        someEvent.Invoke(maxHealth, currentHealth);
    }
}
