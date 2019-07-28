using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTriggerReactor : MonoBehaviour, ITriggerReactor
{
    public bool enableAiDetector;


    public bool TriggerActivated() {
        if (enableAiDetector) {
            AiDetector aiDetector = GetComponent<AiDetector>();
            if (aiDetector) {
                aiDetector.enabled = true;
            }           
        }
        return true;
    }

    public bool TriggerDeactivated() {
        return true;
    }
    
}
