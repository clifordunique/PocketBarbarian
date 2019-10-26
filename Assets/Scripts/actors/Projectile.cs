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
    public bool useTrail = false;
    [ConditionalHideAttribute("useTrail", true)]
    public GameObject trailPrefab;
    public float trailInterval;
    private float lastInterval;
    private Rigidbody2D rigidb;
    [HideInInspector]
    public Vector3 startPosition;
    private Vector3 targetPosition;
    private bool vector;

    [HideInInspector]
    public bool stopMovement = false;

    public virtual void Start() {
        rigidb = GetComponent<Rigidbody2D>();
    }

    public void InitProjectile(Vector3 target, bool vector = false, bool changeLocalScale = true) {
        this.target = target;
        this.startPosition = transform.position;
        this.vector = vector;
        if (((!vector && target.x < transform.position.x) || (vector && target.x < 0)) && changeLocalScale) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        targetPosition = target;
        lastInterval = Time.timeSinceLevelLoad;
    }


    public virtual void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (reactLayers == (reactLayers | (1 << collision.gameObject.layer))) {
            // hit something. Destroy
            Destroy(gameObject);
        }
    }


    void Update() {
        if (!stopMovement) {
            if (!vector && MoveUtils.TargetReachedXY(transform, target, speed, 0)) {
                // if target reached but nothing hit, destroy object
                Destroy(gameObject);
            }

            if (useTrail) {
                if (Time.timeSinceLevelLoad >= lastInterval + trailInterval) {
                    InstantiateEffect(trailPrefab, false);
                    lastInterval = Time.timeSinceLevelLoad;
                }
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

    public void InstantiateEffect(GameObject effectToInstanciate, bool flipSprite) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();
        if (effectSpriteRenderer) {
            if (flipSprite) {
                effectSpriteRenderer.flipX = (transform.localScale.x > 0);
            } else {
                effectSpriteRenderer.flipX = (transform.localScale.x < 0);
            }
        }
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}
