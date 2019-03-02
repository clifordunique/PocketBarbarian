using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHurtBox : HurtBox {


    public LayerMask attackLayersUnDashable;
    public float switchBackDelayTime = 0.1F;

    private LayerMask attackLayersOrigin;
    private bool dashLayerActive = false;

    private static PlayerHurtBox _instance;

    public void Awake() {
        _instance = this;    
    }

    public override void Start() {
        base.Start();
        actorController = GetComponent<PlayerController>();
        if (actorController == null && transform.parent) {
            // search in parent
            actorController = transform.parent.GetComponent<PlayerController>();
        }
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
            StartCoroutine(RefreshCollisions());
        }
    }
    
    public override void ModifyHealth(int healthModifier) {
        base.ModifyHealth(healthModifier);
        ((PlayerController)actorController).statistics.ModifyHealth(healthModifier);
    }
}
