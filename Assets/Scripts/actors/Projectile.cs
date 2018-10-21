using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask reactLayers;
    public float speed;
    public bool moveInArc = false;
    [ConditionalHideAttribute("moveInArc", true)]
    public float arcHeight;

    public Vector3 target;
    private Rigidbody2D rigidb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool vector;

    private void Start() {
        rigidb = GetComponent<Rigidbody2D>();
    }

    public void InitProjectile(Vector3 target, bool vector = false) {
        this.target = target;
        this.startPosition = transform.position;
        this.vector = vector;
        if (target.x < transform.position.x) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        targetPosition = target;
    }


    public void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (reactLayers == (reactLayers | (1 << collision.gameObject.layer))) {
            // hit something. Destroy
            Destroy(gameObject);
        }
    }


    void Update() {
        if (!vector && MoveUtils.TargetReachedXY(transform, target, speed, 0)) {
            // if target reached but nothing hit, destroy object
            Destroy(gameObject);
        }

        if (vector) {
            rigidb.MovePosition(transform.position + (targetPosition * speed * Time.deltaTime));
            //transform.position += targetPosition * speed * Time.deltaTime;
        } else {

            if (moveInArc) {
                // Compute the next position, with arc added in
                float x0 = startPosition.x;
                float x1 = target.x;
                float dist = x1 - x0;
                float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
                float baseY = Mathf.Lerp(startPosition.y, target.y, (nextX - x0) / dist);
                float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);

                targetPosition = new Vector3(nextX, baseY + arc, transform.position.z);
                rigidb.MovePosition(targetPosition);
            } else {
                Vector3 newPos = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                
                rigidb.MovePosition(Utils.MakePixelPerfect(newPos));
            }
        }
    }
}
