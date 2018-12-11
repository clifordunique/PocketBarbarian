using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public LayerMask interactableLayer;
    public float speed;
    public float moveX;

    private bool run = false;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float seconds;
    private Vector3 currentStartPos;
    private Vector3 currentEndpos;
    private float t = 0;
    private float startDelay;

    // Use this for initialization
    void Start () {
        startDelay = Random.Range(0F, 2F);
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        seconds = Mathf.Abs(moveX) * (1/speed);
        currentStartPos = transform.position;
        currentEndpos = new Vector3(transform.position.x + moveX, transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (!run && Time.time > startDelay) {
            animator.SetBool("IDLE", true);
        }

        if (run) {
            Vector2 velocity = SmoothMove();
            Vector2 pixelPerfectVelocity = Utils.MakePixelPerfect(velocity);
            transform.Translate(pixelPerfectVelocity);
        }
		
	}

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {
            animator.SetBool("RUN" ,true);
            run = true;
            boxCollider.enabled = false;
        }
    }


    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / seconds;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = EasingFunction.EaseInOutSine(0.0F, 1.0F, newTime);
            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);

            return newPosition - transform.position;

        }
        run = false;
        animator.SetBool("OUT", true);
        return Vector3.zero;
    }

    public void OutEnd() {
        Destroy(gameObject);
    }


}
