using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Generic Script for handling damage
public class GameActor : MonoBehaviour {

    [Header("Destructability")]
    // is GameObject destructable or just a trap/abstacle
    public bool destructable;

    // layers to react on contact and receive damage
    [ConditionalHide("destructable", true)]
    public LayerMask attackLayers;
    [ConditionalHide("destructable", true)]
    public int maxHealth;
    [ConditionalHide("destructable", true)]
    public int currentHealth;
    // should be overriden for complex death animation, otherwise Game object is destroyed when health below 0
    [ConditionalHide("destructable", true)]
    public bool destroyOnDeathImmediate = true;

    [Header("Damage")]
    // damage on contact
    public int damage;




    // Use this for initialization
    public virtual void Start () {
        currentHealth = maxHealth;
    }

    public virtual void ReactToHit(Vector3 hitDirection) {
        // method should be overriden for complex game actors.
        if (destructable) {
            //TODO Effect Hit
            if (currentHealth <= 0) {
                //TODO Effect Death
                if (destroyOnDeathImmediate) {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // indestructable actors does not react to collision
        if (destructable) {
            // react to hit
            if (attackLayers == (attackLayers | (1 << collision.gameObject.layer))) {

                // getting hit direction
                Vector2 hitDirection = GetHitDirection(collision);

                // get GameActor from collision gameobject
                GameActor attackerActor = collision.gameObject.GetComponent<GameActor>();
                if (attackerActor) {
                    // receive damage from attacker
                    currentHealth -= attackerActor.damage;
                    ReactToHit(hitDirection);
                } else {
                    Debug.Log("Attacker has no GameActor component!");
                }
            }
        }
    }
    

    public Vector3 GetHitDirection(Collision2D collision) {
        Vector3 v = new Vector3(transform.position.x - collision.transform.position.x, transform.position.y - collision.transform.position.y, 1).normalized;
        Vector3 result = new Vector3();
        if (v.x > 0F) result.x = 1;
        if (v.x < -0F) result.x = -1;

        if (v.y > 0.5F) result.y = 1;
        if (v.y < -0.5F) result.y = -1;
        result.y = 0;
        return result;
    }
}

