using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable: MonoBehaviour {

    public LayerMask collectorLayer;
    public float delayTimeCollection = 0;
    public IntValue points;

    public PrefabValue collectableEffect;

    private BoxCollider2D boxCollider;
    private GameObject activeGameObject;
    private bool collected = false;

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
    }

    private void EnableBoxCollider() {
        boxCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to hit
        if (collectorLayer == (collectorLayer | (1 << collider.gameObject.layer))) {
            if (!collected) {
                collected = true;
                if (collectableEffect) {
                    InstantiateEffect(collectableEffect.value);
                }
                Debug.Log("Collected: " + points.value);
                Inventory.GetInstance().AddPoints(points.value);
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