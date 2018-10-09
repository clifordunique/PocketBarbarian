using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public int points;

    private static Inventory _instance;

    // Use this for initialization
    void Start () {
        _instance = this;        
	}

    public static Inventory GetInstance() {
        return _instance;
    }

    public void AddPoints(int additionalPoints) {
        points += additionalPoints;
    }

}
