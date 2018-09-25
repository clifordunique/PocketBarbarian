using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour {

    // layers to react on contact and receive damage
    public LayerMask attackLayers;
    public int maxHealth;
    public int currentHealth;
    
    public bool pushedOnHit;
    public bool flashOnHit;
    // should be overriden for complex death animation, otherwise Game object is destroyed when health below 0
    public bool destroyOnDeathImmediate = true;

    public GameObject prefabHitEffect;
    public GameObject prefabDeathEffect;

    public float hitTime;

    private EnemyController enemyController;
    private BoxCollider2D boxCollider;

    // Use this for initialization
    public virtual void Start() {
        currentHealth = maxHealth;
        boxCollider = GetComponent<BoxCollider2D>();
        enemyController = GetComponent<EnemyController>();
        if (!enemyController && transform.parent) {
            // search in parent
            enemyController = transform.parent.GetComponent<EnemyController>();
        }
    }


    public void ReactToHit(Collision2D collision) {
        if (currentHealth <= 0) {
            if (destroyOnDeathImmediate) {
                DeathAction();
            }            
        } else {
            if (prefabHitEffect != null) {
                InstantiateEffect(prefabHitEffect);
            }
        }

        if (enemyController) {
            enemyController.Hurt(currentHealth <= 0, pushedOnHit, collision.transform.position);
        }

        boxCollider.enabled = false;
        if (flashOnHit) {
            enemyController.FlashSprite(hitTime);
        }
        Invoke("EnableBoxCollider", hitTime);
    }

    public void EnableBoxCollider() {
        boxCollider.enabled = true;

        if (currentHealth <= 0 && !destroyOnDeathImmediate) {
            DeathAction();
        }
    }

    private void DeathAction() {
        if (prefabDeathEffect != null) {
            InstantiateEffect(prefabDeathEffect);
        }
        if (enemyController) {
            Destroy(enemyController.gameObject);
        } else {
            Destroy(gameObject);
        }        
    }


    public void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (attackLayers == (attackLayers | (1 << collision.gameObject.layer))) {
            // get GameActor from collision gameobject
            HitBox attackerActor = collision.gameObject.GetComponent<HitBox>();
            if (attackerActor) {
                // receive damage from attacker
                currentHealth -= attackerActor.damage;
                attackerActor.HitEnemyEvent();
                ReactToHit(collision);
            } else {
                Debug.Log("Attacker has no HitBox component!");
            }
        }        
    }


    public void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }

}
