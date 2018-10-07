using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable: MonoBehaviour {

    public LayerMask collectorLayer;
    public float delayTimeCollection = 0;

    public GameObject collectableEffect;

    private BoxCollider2D boxCollider;


    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider) {
            if (delayTimeCollection > 0) {
                boxCollider.enabled = false;
                Invoke("EnableBoxCollider", delayTimeCollection);
            }            
        }
    }

    private void EnableBoxCollider() {
        boxCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to hit
        if (collectorLayer == (collectorLayer | (1 << collider.gameObject.layer))) {
            Debug.Log("COLLECTED!!!");
            if (collectableEffect) {
                InstantiateEffect(collectableEffect);
            }
            Destroy(transform.parent.gameObject);

        }
    }

    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}