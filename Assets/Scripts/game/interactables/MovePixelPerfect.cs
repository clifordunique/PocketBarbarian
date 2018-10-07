using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePixelPerfect : MonoBehaviour {    
	
	// Update is called once per frame
	void FixedUpdate () {        
       Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(transform.position);
       transform.position = pixelPerfectMoveAmount;
    }
}
