using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJump : MonoBehaviour {
    public Vector2 jumpForce;
    public bool randomDirectionX = false;
    public bool randomDirectionY = false;

    private Rigidbody2D rBody;
    private bool moving = false;

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody2D>();
        if (randomDirectionX) {
            jumpForce.x = Random.Range(-jumpForce.x, jumpForce.x);
        }
        if (randomDirectionY) {
            jumpForce.y = jumpForce.y - Random.Range(-1F, 1F);
        }

        rBody.velocity = new Vector2(jumpForce.x, jumpForce.y);
    }
	
	void Update () {
        
     /*   if (rBody.IsTouchingLayers()) {
            moving = false;
        }
        if (!moving && Input.GetKeyDown(KeyCode.C)) {
            rBody.velocity = new Vector2(jumpForce.x, jumpForce.y);
            moving = true;
        }
        */
    }
}
