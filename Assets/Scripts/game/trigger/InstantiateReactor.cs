using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateReactor: MonoBehaviour, ITriggerReactor {

    public GameObject prefab;
    public bool customPosition;
    [ConditionalHideAttribute("customPosition", true)]
    public Vector3 position;

    
    public virtual bool TriggerActivated() {
        if (customPosition) {
            Instantiate(prefab, position, transform.rotation);
        } else {
            Instantiate(prefab, transform.position, transform.rotation);
        }
        
        return true;
    }

    public bool TriggerDeactivated() {
        return true;
    }
}
