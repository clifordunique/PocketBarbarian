using UnityEngine;
using System.Collections.Generic;
using System;

public class UniqueId: MonoBehaviour {

    public string uniqueId;

    public void Start() {
        // create uniqueId
        Vector3 pos = transform.position;
        uniqueId = "ID_";
        uniqueId += pos.x + "_" + pos.y + "_" + pos.z;
    }
}