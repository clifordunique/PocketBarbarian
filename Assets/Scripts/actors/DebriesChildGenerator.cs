using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriesChildGenerator : MonoBehaviour, IChildGenerator {

    public int randomRangeFrom;
    public int randomRangeTo;
    public GameObject debriesPrefab;

    public void SpawnChildrenOnDeath() {
        int count = Random.Range(randomRangeFrom, randomRangeTo + 1);
        for(int i = 0; i < count; i++) {
            InstantiateDebries(debriesPrefab);
        }
    }

    public void SpawnChildrenOnHit() {
        // not implemented
    }


    private void InstantiateDebries(GameObject toInstanciate) {
        GameObject effect = (GameObject)Instantiate(toInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}
