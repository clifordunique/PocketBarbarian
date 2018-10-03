using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePixelPerfect : MonoBehaviour {    
	
	// Update is called once per frame
	void LateUpdate () {        
       Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(transform.position);
       transform.position = pixelPerfectMoveAmount;
    }
}
