using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnCollisionEnter2D(Collision2D collision) {

        Debug.Log("CollisionEnter2");

        float directionX = GetHitDirection(transform, collision);
        Debug.Log("Hit: " + directionX);
    }

    public static float GetHitDirection(Transform thisTransform, Collision2D collision) {

        if (thisTransform) {
            if (collision.transform.position.x > thisTransform.position.x) {
                // attack from the left
                return -1;
            }
            if (collision.transform.position.x < thisTransform.position.x) {
                // attack from the right
                return 1;
            }
        }

        return 0;
    }
}
