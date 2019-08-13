using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticCurveMovement : MonoBehaviour
{
    // layers to react on contact and receive damage
    public LayerMask reactLayers;

    public float speed = 1F;
    public float targetCheckInterval = 0.5F;
    private float lastDeltaTime;
    private float lastDistance = -1;
    private float f;
    private bool move = false;
    private float directionX = 1F;   // -1 left, 1 right
    private float directionY = -1F;  // -1 down, 1 up
    private float targetX;
    private float targetY;

    private Transform target;

    private Camera cam;
    private BoxCollider2D bCollider;
    private SpriteRenderer sRenderer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        bCollider = GetComponent<BoxCollider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (reactLayers == (reactLayers | (1 << collider.gameObject.layer))) {

            anim.SetBool("START", true);

            target = collider.gameObject.transform;

            // check if moving to the right or left
            if (target.position.x > transform.position.x) {
                // to the right
                directionX = 1F;
                sRenderer.flipX = true;
            } else {
                // to the left
                directionX = -1F;
                sRenderer.flipX = false;
            }

            CalculateFormular();

            Vector3 newV = GetNewLocation(Time.deltaTime);
            lastDistance = Vector3.Distance(transform.position, newV);
            lastDeltaTime = Time.deltaTime;
            move = true;

            // Recalculate Target
            StartCoroutine(RecalcTarget());

            // BoxCollider brauchen wir nicht mehr
            bCollider.enabled = false;
        }
    }

    IEnumerator RecalcTarget() {
        while (directionY == -1F) {
            yield return new WaitForSeconds(targetCheckInterval);
            Debug.Log("Recalc");
            CalculateFormular();            
        }
    }

    private void CalculateFormular() {
        if (transform.position.y > target.position.y - 0.5f && !HasTargetPassed()) {
            targetX = target.position.x;
            targetY = target.position.y - 0.5f;
            f = (transform.position.y - targetY) / (Mathf.Pow(transform.position.x - targetX, 2));
        }
    }

    private bool HasTargetPassed() {
        if ((directionX == 1 && target.position.x <= transform.position.x) ||
            (directionX == -1 && target.position.x >= transform.position.x)) {
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (move && Time.deltaTime > 0) {
            float newDistance;
            Vector3 newV;
            float lastDistPerSecond = lastDistance / lastDeltaTime;
            Vector3 refV = GetNewLocation(Time.deltaTime);
            newDistance = Vector3.Distance(transform.position, refV);
            //Debug.Log("Delta:" + Time.deltaTime);
            float currentDistPeSecond = newDistance / Time.deltaTime;
            float calcFactor = lastDistPerSecond / currentDistPeSecond;

            //Debug.Log("currentDistPeSecond:" + currentDistPeSecond + " / lastDistPerSecond: " + lastDistPerSecond + " / Factor: " + calcFactor + " / lastDelta: " + lastDeltaTime);

            newV = GetNewLocationFactor(calcFactor);

            newDistance = Vector3.Distance(transform.position, newV);
            //Debug.Log("NewDist: " + newDistance);
            
            if (transform.position.y > newV.y) {
                // moving down
                Debug.Log("Moving down");
                directionY = -1F;
            } else {
                // moving up
                Debug.Log("Moving up");
                directionY = 1F;
            }
            transform.position = newV;

            // Check if visible
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
            if (directionY == 1 && (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)) {
                Debug.Log("Out of view!");
                Destroy(gameObject);
            } 
        }
    }

    private Vector3 GetNewLocationFactor(float xFactor) {
        return GetNewLocation(Time.deltaTime, xFactor);
    }

    private Vector3 GetNewLocation(float timeFactor, float xFactor = 1) {
        float newX = (transform.position.x - targetX) + (directionX * (timeFactor * speed) * xFactor);        
        float newY = f * (Mathf.Pow(newX, 2));
        Vector3 newV = new Vector3(newX + targetX, newY + targetY, 0F);
        return newV;
    }
}
