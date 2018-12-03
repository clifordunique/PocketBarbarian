using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstactLockedInteractable : AbstactInteractable {
    
    public GameObject actionArrowLocked;
    public GameObject lockSymbol;
    public GameObject keySymbol;        
    public CollectableKeys.KEY_TYPE lockedKey;    
    public float unlockTime = 1F;
    public float waitAfterUnlockTime = 0.2F;

    private BoxCollider2D boxCollider;
    [HideInInspector]
    public bool open;

    public override void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        if (Unlockable()) {
            lockSymbol.SetActive(false);
            keySymbol.SetActive(true);
            boxCollider.enabled = true;
        } else {
            lockSymbol.SetActive(true);
            keySymbol.SetActive(false);
            boxCollider.enabled = false;
        }
    }

    public void Update() {
        if (Time.frameCount % 5 == 0) {
            if (!open) {
                if (Unlockable()) {
                    lockSymbol.SetActive(false);
                    keySymbol.SetActive(true);
                    boxCollider.enabled = true;
                } else {
                    lockSymbol.SetActive(true);
                    keySymbol.SetActive(false);
                    boxCollider.enabled = false;
                }
            }
        }
    }
    

    public void Unlock() {
        if (Unlockable()) {
            StartCoroutine(UnlockCoroutine());
        } 
    }

    IEnumerator UnlockCoroutine() {        
        SpriteRenderer spriteRenderer = actionArrowLocked.GetComponent<SpriteRenderer>();
        SpriteFlashingEffect effect = new SpriteFlashingEffect();
        StartCoroutine(effect.ActionFlashing(spriteRenderer, unlockTime));
        yield return new WaitForSeconds(unlockTime);
        actionArrowLocked.SetActive(false);
        yield return new WaitForSeconds(waitAfterUnlockTime);
        
        keySymbol.SetActive(false);
        actionArrowLocked.SetActive(false);
        actionArrow.SetActive(true);
        open = true;
    }


    public override void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                if (open) {
                    actionArrow.SetActive(true);
                }
                if (!open && Unlockable()) {
                    actionArrowLocked.SetActive(true);
                } 
            }
        }
    }

    public override void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrowLocked.SetActive(false);
                actionArrow.SetActive(false);
            }
        }
    }

    public bool Unlockable() {
        PlayerStatistics ps = PlayerStatistics.GetInstance();
        if (ps) {
            if (lockedKey == CollectableKeys.KEY_TYPE.CIRCLE && ps.hasCircleKey) {
                return true;
            }
            if (lockedKey == CollectableKeys.KEY_TYPE.SQUARE && ps.hasSquareKey) {
                return true;
            }
            if (lockedKey == CollectableKeys.KEY_TYPE.TRIANGLE && ps.hasTriangleKey) {
                return true;
            }
        }
        return false;
    }
}