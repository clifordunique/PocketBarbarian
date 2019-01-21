using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCollectable : MonoBehaviour {

    public LayerMask collectorLayer;
    public float delayTimeCollection = 0;
    public PrefabValue collectableEffect;
    public bool startRandom = true;

    private BoxCollider2D boxCollider;
    private GameObject activeGameObject;
    private bool collected = false;

    private Animator animator;
    private SimpleMovement movement;

    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider) {
            if (delayTimeCollection > 0) {
                boxCollider.enabled = false;
                Invoke("EnableBoxCollider", delayTimeCollection);
            }
        }

        if (GetComponent<SpriteRenderer>()) {
            activeGameObject = gameObject;
        } else {
            activeGameObject = transform.parent.gameObject;
        }

        movement = GetComponent<SimpleMovement>();
        animator = GetComponent<Animator>();
        if (!animator && transform.parent != null) {
            animator = transform.parent.GetComponent<Animator>();
        }

        if (startRandom) {
            if (movement) movement.enabled = false;
            if (animator) animator.enabled = false;
            float randomStartTime = Time.time + Random.Range(0F, 1F);
            StartCoroutine(StartAnimation(randomStartTime));
        }
    }

    IEnumerator StartAnimation(float time) {
        yield return new WaitForSeconds(time);
        if (animator) {
            animator.enabled = true;
        }
        if (movement) {
            movement.enabled = true;
        }
    }

    private void EnableBoxCollider() {
        boxCollider.enabled = true;
    }

    public abstract void CollectItem();

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to hit
        if (collectorLayer == (collectorLayer | (1 << collider.gameObject.layer))) {
            if (!collected) {
                collected = true;
                if (collectableEffect) {
                    InstantiateEffect(collectableEffect.value);
                }
                CollectItem();
                Destroy(activeGameObject);
            }
        }
    }

    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}
