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
    public GameObject prefabWaterSplash;

    public float hitTime;
    [HideInInspector]
    public IActorController actorController;
    private GameObject actorGameObject;
    [HideInInspector]
    public BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private IChildGenerator lootController;
    
    private bool hitInProgress = false;

    // Use this for initialization
    public virtual void Start() {        
        boxCollider = GetComponent<BoxCollider2D>();
        actorController = GetComponent<IActorController>();
        if (actorController == null && transform.parent) {
            // search in parent
            actorController = transform.parent.GetComponent<IActorController>();
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

        lootController = GetComponent<IChildGenerator>();
        if (lootController == null && transform.parent) {
            // search in parent
            lootController = transform.parent.GetComponent<IChildGenerator>();
        }

        ModifyHealth(0);
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
            HitBox attackerActor = collision.transform.GetComponent<HitBox>();
            if (attackerActor) {
                ReceiveHit(attackerActor.instakill, attackerActor.damage, attackerActor.damageType, attackerActor.transform, collision.collider);
            } else {
                Debug.Log("Attacker has no HitBox component!");
            }
        }
    }

    public void ReceiveHit(bool instakill, int damage, HitBox.DAMAGE_TYPE damageType, Transform hitSource, Collider2D collider2d) {
        if (currentHealth > 0) {
            if (instakill) {
                //instakill
                Debug.Log("HurtBox collider = false");
                boxCollider.enabled = false;
            }
            if (!hitInProgress || instakill) {
                // receive damage from attacker
                if (instakill) {
                    ModifyHealth(-currentHealth);
                } else {
                    ModifyHealth(-damage);
                }
                HitBox attackerActor = hitSource.GetComponent<HitBox>();
                if (attackerActor) {
                    attackerActor.ReactHit();
                }
                ReactHurt(collider2d, hitSource, instakill, damage, damageType);
            }
        }
    }
        

    private void ReactHurt(Collider2D collider2d, Transform hitSource, bool instakill, int damage, HitBox.DAMAGE_TYPE damageType) {
        Vector3 hitDirection = Utils.GetHitDirection(hitSource.position, transform);

        if (currentHealth <= 0) {
            if (destroyOnDeathImmediate) {
                DeathAction(hitDirection.x);
            }            
        } else {
            if (prefabHitEffect != null) {                
                InstantiateEffect(prefabHitEffect, hitDirection.x, transform.position);
            }
        }


        if (damageType == HitBox.DAMAGE_TYPE.WATER && instakill && collider2d != null) {
            float y = collider2d.bounds.center.y + collider2d.bounds.extents.y;
            Vector3 effectPosition = new Vector3(transform.position.x, y, transform.position.z);
            InstantiateEffect(prefabWaterSplash, hitDirection.x, effectPosition, hitSource);
        }
        if (actorController != null) {
            actorController.ReactHurt(currentHealth <= 0, this.pushedOnHit, instakill, hitSource.position, damageType);
        }

        // Spawn Effect on hit
        if (currentHealth > 0 && lootController != null) {
            lootController.SpawnChildrenOnHit();
        }

        if (flashOnHit && !instakill) {
            FlashSprite(flashTime);
        }
        if (hitTime > 0 || currentHealth <= 0) {
            hitInProgress = true;
        }
        if (currentHealth <= 0 && !destroyOnDeathImmediate) {
            StartCoroutine(CoroutineDeath(hitDirection.x, flashTime));
        }
        if (hitTime > 0 && currentHealth > 0) {
            // enable box collider if still living
            Invoke("HitInProgress", hitTime);
        }
    }

    private void HitInProgress() {
        hitInProgress = false;
    }

    IEnumerator CoroutineDeath(float hitDirectionX, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        DeathAction(hitDirectionX);
    }

    private void DeathAction(float hitDirectionX) {
        if (prefabDeathEffect != null) {
            InstantiateEffect(prefabDeathEffect, hitDirectionX, transform.position);
        }

        // Spawn Effect on death
        if (lootController != null) {
            lootController.SpawnChildrenOnDeath();
        }

        if (destroyOnDeath) {
            Destroy(actorGameObject);
        }
    }

    private void InstantiateEffect(GameObject effectToInstanciate, float dirX, Vector3 position, Transform parent = null) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        if (parent == null) {
            effect.transform.parent = EffectCollection.GetInstance().transform;
        } else {
            effect.transform.parent = parent;
        }
        effect.transform.position = position;
        effect.transform.localScale = new Vector3(dirX, effect.transform.localScale.y, effect.transform.localScale.z);
    }

    private void FlashSprite(float time) {
        SpriteFlashingEffect effect = new SpriteFlashingEffect();
        StartCoroutine(effect.DamageFlashing(spriteRenderer, time));
    }

}
