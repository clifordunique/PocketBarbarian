using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeProjectile : Projectile {

    public float delayDestroy;
    public GameObject prefabHitEffect;

    private Animator animator;
    private BoxCollider2D boxCollider;


    public override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if (reactLayers == (reactLayers | (1 << collision.gameObject.layer))) {            
            // hit something. 
            Vector2 hitPosition = transform.position;

            // check if axe traveled or already hit something
            float distance = Vector3.Distance(startPosition, transform.position);
            if (startPosition == transform.position || distance <= 0.1F) {
                hitPosition = new Vector2(collision.GetContact(0).point.x, transform.position.y);
            }
            
            stopMovement = true;
            animator.SetBool("STOP", true);
            
            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(hitPosition);
            transform.position = pixelPerfectMoveAmount;
            transform.parent = collision.gameObject.transform;
            boxCollider.enabled = false;
            InstantiateEffect(prefabHitEffect);
            StartCoroutine(DeleteDelayed());
        }
    }

    IEnumerator DeleteDelayed() {
        yield return new WaitForSeconds(delayDestroy);
        Destroy(gameObject);
    }

    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();
        if (effectSpriteRenderer) {
            effectSpriteRenderer.flipX = (transform.localScale.x > 0);
        }
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}
