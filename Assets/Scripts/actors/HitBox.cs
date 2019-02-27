using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    [Header("Damage")]
    // damage on contact
    public int damage;

    public enum DAMAGE_TYPE { DEFAULT, DASH, WATER, SQUISH};
    public DAMAGE_TYPE damageType;
    public bool instakill = false;

    private IActorController actorController;

    public void Start() {
        actorController = GetComponent<IActorController>();
        if (actorController == null && transform.parent != null) {
            // search in parent
            actorController = transform.parent.GetComponent<IActorController>();
        }
    }

    public void ReactHit() {
        if (actorController != null) {
            actorController.ReactHit();
        }
    }
}
