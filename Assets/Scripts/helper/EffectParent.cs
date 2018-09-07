using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParent : MonoBehaviour {

    // Static instance
    static GameObject _instance;


    public static GameObject GetInstance() {
        if (!_instance) {
            _instance = new GameObject("EffectParent");
        }
        return _instance;
    }
}
