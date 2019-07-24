using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour, ITriggerReactor {

    public LeverInteractable[] switches;
    public bool[] order;

    private TriggerManager triggerManager;

    // Start is called before the first frame update
    void Start()
    {
        if (switches.Length != order.Length) {
            Debug.LogError("Length switches not length order");
        }

        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    private IEnumerator CheckState() {
        yield return 0;
        //Debug.Log("Checkstate");
        int i = 0;
        bool puzzleSolved = true;
        foreach(LeverInteractable it in switches) {
            bool switchStateExpected = order[i];
            //Debug.Log("Switch: " + it.name + " : " + it.activated);
            //Debug.Log("Order: " + i + " : " + switchStateExpected);
            if (it.activated != switchStateExpected) {
                puzzleSolved = false;
            }
            i++;
        }
        if (puzzleSolved) {
            Debug.Log("PUZZLE SOLVED!");
            foreach (LeverInteractable it in switches) {
                it.Disable();
            }
            triggerManager.ActivateReactors();
        } else {
            triggerManager.DeactivateReactors();
        }
    }


    public bool TriggerActivated() {
        StartCoroutine(CheckState());
        return true;
    }

    public bool TriggerDeactivated() {
        StartCoroutine(CheckState());
        return true;
    }
}
