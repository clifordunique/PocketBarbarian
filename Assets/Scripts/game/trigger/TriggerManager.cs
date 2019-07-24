using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public bool useExternalTriggerReactors = false;
    public GameObject[] reactorGameObjects;

    private List<ITriggerReactor> triggerReactors = new List<ITriggerReactor>();

    public void Start() {
        if (!useExternalTriggerReactors) {
            ITriggerReactor reactor = GetComponent<ITriggerReactor>();
            if (reactor != null) {
                triggerReactors.Add(reactor);
            }
        } else {
            // check if Gameobjects all have an interface ITriggerReactor as Component
            foreach(GameObject go in reactorGameObjects) {
                ITriggerReactor reactor = go.GetComponent<ITriggerReactor>();
                if (reactor != null) {
                    triggerReactors.Add(reactor);
                } else {
                    Debug.LogError("TriggerManager " + gameObject.name + ": reactorGameObject not of Interface ITriggerReactor: " + go.name);
                }
            }
        }


    }

    public bool ActivateReactors() {
        if (triggerReactors != null && triggerReactors.Count > 0) {
            bool allTriggersSuccessfull = true;
            foreach(ITriggerReactor reactor in triggerReactors) {
                if (!reactor.TriggerActivated()) {
                    allTriggersSuccessfull = false;
                }
            }
            return allTriggersSuccessfull;
        }
        return false;
    }


    public bool DeactivateReactors() {
        if (triggerReactors != null && triggerReactors.Count > 0) {
            bool allTriggersSuccessfull = true;
            foreach (ITriggerReactor reactor in triggerReactors) {
                if (!reactor.TriggerDeactivated()) {
                    allTriggersSuccessfull = false;
                }
            }
            return allTriggersSuccessfull;
        }
        return false;
    }

    public bool HasReactors() {
        if (triggerReactors.Count > 0) {
            return true;
        }
        return false;
    }
}
