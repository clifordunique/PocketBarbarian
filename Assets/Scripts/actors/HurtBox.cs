using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour {

    // layers to react on contact and receive damage
    public LayerMask attackLayers;
    public int maxHealth;
    public int currentHealth;
    
    public bool pushedOnHit;
    public bool blinkOnHit;
    public float hitDuration;
    // should be overriden for complex death animation, otherwise Game object is destroyed when health below 0
    public bool destroyOnDeathImmediate = true;

    private EnemyStateMachine stateMachine;

    // Use this for initialization
    public virtual void Start() {
        currentHealth = maxHealth;
        stateMachine = GetComponent<EnemyStateMachine>();
    }


    public void ReactToHit(Collision2D collision) {
        //TODO Effect Hit
        if (currentHealth <= 0) {
            //TODO Effect Death
            if (destroyOnDeathImmediate) {
                Destroy(gameObject);
            }
        }

        if (stateMachine) {
            // interrupt stateMachine with hit
            EnemyAction hitAction = new EnemyAction(EnemyAction.ACTION_EVENT.HIT);
            hitAction.amount = hitDuration;
            if (pushedOnHit) {
                hitAction.hitTarget = collision.transform.position;
            } else {
                hitAction.hitTarget = Vector3.positiveInfinity;
            }
            stateMachine.InterruptAction(hitAction);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (attackLayers == (attackLayers | (1 << collision.gameObject.layer))) {

            // getting hit direction
            //Vector2 hitDirection = GetHitDirection(collision);

            // get GameActor from collision gameobject
            HitBox attackerActor = collision.gameObject.GetComponent<HitBox>();
            if (attackerActor) {
                // receive damage from attacker
                currentHealth -= attackerActor.damage;
                ReactToHit(collision);
            } else {
                Debug.Log("Attacker has no HitBox component!");
            }
        }        
    }
    
}
