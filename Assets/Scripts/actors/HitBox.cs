using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    [Header("Damage")]
    // damage on contact
    public int damage;

    public enum DAMAGE_TYPE { DEFAULT};
    public DAMAGE_TYPE damageType;

    private PlayerController playerController;

    public void Start() {
        playerController = GetComponent<PlayerController>();
        if (!playerController) {
            // search in parent
            playerController = transform.parent.GetComponent<PlayerController>();
        }
    }

    public void HitEnemyEvent() {
        if (playerController) {
            playerController.InterruptEvent("HIT ENEMY");
        }
    }
}
