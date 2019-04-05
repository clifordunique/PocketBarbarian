using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTriggerReactor : MonoBehaviour, ITriggerReactor {

    public bool enableOnActivated = false;
    public float timeOffset = 0F;
    public GameObject[] activatableGameObjects;

    public bool TriggerActivated() {

        foreach(GameObject go in activatableGameObjects) {
            StartCoroutine(DoIt(go, enableOnActivated));
        }

        return true;
    }

    public bool TriggerDeactivated() {

        foreach (GameObject go in activatableGameObjects) {
            StartCoroutine(DoIt(go, !enableOnActivated));
        }

        return true;
    }

    IEnumerator DoIt(GameObject go, bool active) {
        yield return new WaitForSeconds(timeOffset);
        go.SetActive(active);
    }
    
}
