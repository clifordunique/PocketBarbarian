using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public string uuid;
    public bool activated = false;
    public float spawnOffsetY = -0.75F;
    public LayerMask reactLayer;

    private Animator animator;
    private BoxCollider2D boxCollider;   

    void Awake() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public Vector3 GetSpawnPosition() {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + spawnOffsetY, transform.position.z);
        return spawnPos;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!activated) {
                Activate();
            }
        }
    }

    public void Activate() {
        activated = true;
        if (animator) {
            animator.SetBool("ACTIVATE", true);
            animator.SetBool("DEACTIVATE", false);
        }
        GameManager.GetInstance().SaveGame();
    }

    public void DeActivate() {
        if (animator) {
            animator.SetBool("ACTIVATE", false);
            animator.SetBool("DEACTIVATE", true);
        }
    }
}
