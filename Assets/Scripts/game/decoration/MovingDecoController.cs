using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDecoController : MonoBehaviour
{
    // layers to react on contact and receive damage
    public LayerMask attackLayers;

    private BoxCollider2D boxCollider;
    private float lastColliderX;
    private float boxColliderCenter;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxColliderCenter = boxCollider.bounds.center.x;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (attackLayers == (attackLayers | (1 << col.gameObject.layer))) {
            lastColliderX = col.bounds.center.x;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (attackLayers == (attackLayers | (1 << col.gameObject.layer))) {
            float currentColliderX = col.bounds.center.x;
            if (currentColliderX > lastColliderX) {
                if (lastColliderX < boxColliderCenter && currentColliderX >= boxColliderCenter) { 
                    Debug.Log("Moving to the left over the middle");
                    anim.SetTrigger("RIGHT");
                }
            } 
            else if (col.bounds.center.x < lastColliderX) {
                if (lastColliderX > boxColliderCenter && currentColliderX <= boxColliderCenter) {
                    Debug.Log("Moving to the right over the middle");
                    anim.SetTrigger("LEFT");
                }
            }
            lastColliderX = col.bounds.center.x;
            
        }
    }
}
