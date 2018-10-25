using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDelayEffect: MonoBehaviour {

    public float delayTime;
    public bool withRandomOffset;
    public float randomOffsetDelayTime;

    private float startTime;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        if (withRandomOffset) {
            startTime += Random.Range(0, randomOffsetDelayTime);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if ((startTime + delayTime) < Time.time) {
            Destroy(gameObject);
        }
	}
}
