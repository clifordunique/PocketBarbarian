using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public bool activated = false;
    public float spawnOffsetY = -0.75F;
    public LayerMask reactLayer;

    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
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

    private void Activate() {
        SetActive(true);
        GameManager.GetInstance().SaveGame(this);
    }

    public void SetActive(bool active) {
        activated = active;
        if (animator) {
            animator.SetBool("ACTIVATE", active);
            animator.SetBool("DEACTIVATE", !active);
        }
    }

}
