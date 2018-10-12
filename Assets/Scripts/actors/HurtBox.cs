using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HurtBox : MonoBehaviour {

    // layers to react on contact and receive damage
    public LayerMask attackLayers;
    public int maxHealth;
    public int currentHealth = 0;
    
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
    [HideInInspector]
    public BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private LootController lootController;

    // Use this for initialization
    public virtual void Start() {
        ModifyHealth(maxHealth);
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

        lootController = GetComponent<LootController>();
        if (!lootController && transform.parent) {
            // search in parent
            lootController = transform.parent.GetComponent<LootController>();
        }
    }

    public virtual void ModifyHealth(int healthModifier) {
        if ((currentHealth + healthModifier) > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth = currentHealth + healthModifier;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (attackLayers == (attackLayers | (1 << collision.gameObject.layer))) {
            // get GameActor from collision gameobject
            HitBox attackerActor = collision.gameObject.GetComponent<HitBox>();
            if (attackerActor) {
                // receive damage from attacker
                ModifyHealth(-attackerActor.damage);
                attackerActor.ReactHit();
                ReactHurt(collision);
            } else {
                Debug.Log("Attacker has no HitBox component!");
            }
        }
    }


    private void ReactHurt(Collision2D collision) {
        Vector3 hitDirection = Utils.GetHitDirection(collision.transform.position, transform);

        if (currentHealth <= 0) {
            if (destroyOnDeathImmediate) {
                DeathAction(hitDirection.x);
            }            
        } else {
            if (prefabHitEffect != null) {                
                InstantiateEffect(prefabHitEffect, hitDirection.x);
            }
        }

        if (enemyController != null) {
            enemyController.ReactHurt(currentHealth <= 0, pushedOnHit, collision.transform.position);
        }

        // Spawn loot hit
        if (currentHealth > 0 && lootController && lootController.lootMin) {
            lootController.SpawnLootMin();
        }


        if (flashOnHit) {
            FlashSprite(flashTime);
        }
        if (hitTime > 0 || currentHealth <= 0) {
            boxCollider.enabled = false;
        }
        if (currentHealth <= 0 && !destroyOnDeathImmediate) {
            StartCoroutine(CoroutineDeath(hitDirection.x, flashTime));
        }
        if (hitTime > 0 && currentHealth > 0) {
            // enable box collider if still living
            Invoke("EnableBoxCollider", hitTime);
        }
    }

    private void EnableBoxCollider() {
        boxCollider.enabled = true;
    }

    IEnumerator CoroutineDeath(float hitDirectionX, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        DeathAction(hitDirectionX);
    }

    private void DeathAction(float hitDirectionX) {
        if (prefabDeathEffect != null) {
            InstantiateEffect(prefabDeathEffect, hitDirectionX);
        }

        // Spawn loot hit
        if (lootController) {
            lootController.SpawnLootMax();
        }

        if (destroyOnDeath) {
            Destroy(actorGameObject);
        }
    }

    private void InstantiateEffect(GameObject effectToInstanciate, float dirX) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
        effect.transform.localScale = new Vector3(dirX, effect.transform.localScale.y, effect.transform.localScale.z);
    }

    private void FlashSprite(float time) {
        SpriteFlashingEffect effect = new SpriteFlashingEffect();
        StartCoroutine(effect.DamageFlashing(spriteRenderer, time));
    }

}
