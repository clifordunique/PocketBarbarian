using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItemSpriteAnim : MonoBehaviour, IMenueItemSprite {

    public float duration = 1f;
    public float speed = 1f;
    public float amount = 1f;

    Vector2 startingPos;
    
    private Animator animator;
    private float startTime = -1F;

    public void Start() {
        animator = GetComponent<Animator>();
    }

    public void SetEnabled() {
        animator.SetBool("ENABLED", true);
        animator.SetBool("DISABLED", false);
        StartShake();
    }

    public void SetDisabled() {
        animator.SetBool("ENABLED", false);
        animator.SetBool("DISABLED", true);
        animator.SetBool("CLICKED", false);
    }

    public void Click() {
        animator.SetBool("ENABLED", false);
        animator.SetBool("DISABLED", false);
        animator.SetBool("CLICKED", true);
    }

    private void StartShake() {
        if (startTime == -1) {
            // Position nur beim ersten mal merken
            startingPos.x = transform.position.x;
            startingPos.y = transform.position.y;
        }
        startTime = Time.time;
        StartCoroutine(ShakeMenueItem());
    }
    IEnumerator ShakeMenueItem() {
        while (Time.time <= (startTime + duration)) {
            transform.position = new Vector2(startingPos.x, (startingPos.y + (Mathf.Sin(Time.time * speed) * amount)));
            yield return new WaitForEndOfFrame();
        }
        transform.position = startingPos;
    }

    public float GetWidth() {
        return Utils.GetWidthFromSpriteGO(gameObject);
    }
}
