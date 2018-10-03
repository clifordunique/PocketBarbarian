using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HurtBox : MonoBehaviour {

    // layers to react on contact and receive damage
    public LayerMask attackLayers;
    public int maxHealth;
    public int currentHealth;
    
    public bool pushedOnHit;
    public bool flashOnHit;
    public float flashTime;
    public bool destroyParent = false;
    public bool destroyOnDeath = true;
    public bool destroyOnDeathImmediate = true;

    public GameObject prefabHitEffect;
    public GameObject prefabDeathEffect;

    public float hitTime;

    private IActorController enemyController;
    private GameObject actorGameObject;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    public virtual void Start() {
        currentHealth = maxHealth;
        boxCollider = GetComponent<BoxCollider2D>();
        enemyController = GetComponent<IActorController>();
        if (enemyController == null && transform.parent) {
            // search in parent
            enemyController = transform.parent.GetComponent<IActorController>();
        } 

        actorGameObject = gameObject;
        if (destroyParent) {
            actorGameObject = transform.parent.gameObject;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer && transform.parent) {
            // search in parent
            spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
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
                attackerActor.ReactHit();
                ReactHurt(collision);
            } else {
                Debug.Log("Attacker has no HitBox component!");
            }
        }
    }


    private void ReactHurt(Collision2D collision) {
        if (currentHealth <= 0) {
            if (destroyOnDeathImmediate) {
                DeathAction();
            }            
        } else {
            if (prefabHitEffect != null) {
                InstantiateEffect(prefabHitEffect);
            }
        }

        if (enemyController != null) {
            enemyController.ReactHurt(currentHealth <= 0, pushedOnHit, collision.transform.position);
        }


        if (flashOnHit) {
            FlashSprite(flashTime);
        }
        if (hitTime > 0 || currentHealth <= 0) {
            boxCollider.enabled = false;
        }
        if (currentHealth <= 0 && !destroyOnDeathImmediate) {
            Invoke("DeathAction", flashTime);
        }
        if (hitTime > 0 && currentHealth > 0) {
            // enable box collider if still living
            Invoke("EnableBoxCollider", hitTime);
        }
    }

    private void EnableBoxCollider() {
        boxCollider.enabled = true;
    }



    private void DeathAction() {
        if (prefabDeathEffect != null) {
            InstantiateEffect(prefabDeathEffect);
        }
        if (destroyOnDeath) {
            Destroy(actorGameObject);
        }
    }

    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }

    private void FlashSprite(float time) {
        SpriteFlashingEffect effect = new SpriteFlashingEffect();
        StartCoroutine(effect.DamageFlashing(spriteRenderer, time));
    }

}
