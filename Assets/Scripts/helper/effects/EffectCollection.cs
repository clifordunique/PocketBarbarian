using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCollection : MonoBehaviour {

    // Static instance
    static GameObject _instance;


    public static GameObject GetInstance() {
        if (!_instance) {
            _instance = new GameObject("EffectCollection");
        }
        return _instance;
    }
}
