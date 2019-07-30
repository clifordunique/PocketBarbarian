using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable: AbstactInteractable {

    private const string OPEN_PARAM = "OPEN";
    private Animator animator;
    private BoxCollider2D boxCollider;
    private LootChildGenerator lootController;

    public override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        lootController = GetComponent<LootChildGenerator>();
    }

    public override void Activate() {
        actionArrow.SetActive(false);
        permanentDisabled = true; // once open, no new action possible
        boxCollider.enabled = false;
        animator.SetBool(OPEN_PARAM, true);
    }
    

    public void Opened() {
        actionFinished = true;
        lootController.SpawnChildrenOnDeath();
    }
}
