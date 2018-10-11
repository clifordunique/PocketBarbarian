﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : AbstactInteractable {

    private const string OPEN_PARAM = "OPEN";
    private Animator animator;
    private BoxCollider2D boxCollider;
    private LootController lootController;

    public void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        lootController = GetComponent<LootController>();
    }

    public override void Activate() {
        actionArrow.SetActive(false);
        permanentDisabled = true; // once open, no new action possible
        boxCollider.enabled = false;
        animator.SetBool(OPEN_PARAM, true);
    }
    

    public void Opened() {
        actionFinished = true;
        lootController.SpawnLootMax();
    }
}
