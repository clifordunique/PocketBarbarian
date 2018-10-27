using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    [Header("Damage")]
    // damage on contact
    public int damage;

    public enum DAMAGE_TYPE { DEFAULT};
    public DAMAGE_TYPE damageType;

    private IActorController actorController;

    public void Start() {
        actorController = GetComponent<IActorController>();
        if (actorController == null) {
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
