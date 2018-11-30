using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstactInteractable : MonoBehaviour {


    public GameObject actionArrow;
    public GameObject actionArrowLocked;
    public GameObject lockSymbol;

    public LayerMask reactLayer;

    public bool locked;
    [ConditionalHideAttribute("locked", true)]
    public CollectableKeys.KEY_TYPE lockedKey;
    public bool permanentDisabled = false;
    
    [HideInInspector]
    public bool actionFinished = false;

    public virtual void Start() {
        if (Unlockable()) {
            lockSymbol.SetActive(false);
        } else {
            lockSymbol.SetActive(true);
        }
    }

    public void Update() {
        if (Time.frameCount % 5 == 0) {
            if (locked) {
                if (Unlockable()) {
                    lockSymbol.SetActive(false);
                } else {
                    lockSymbol.SetActive(true);
                }
            }
        }
    }

    public abstract void Activate();

    public void Unlock() {
        if (locked && Unlockable()) {
            locked = false;
            actionArrowLocked.SetActive(false);
            actionArrow.SetActive(true);
        }
    }


    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                if (locked) {                    
                    if (Unlockable()) {
                        actionArrowLocked.SetActive(true);
                    }
                } else {
                    actionArrow.SetActive(true);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrowLocked.SetActive(false);
                actionArrow.SetActive(false);
            }
        }
    }

    public bool Unlockable() {
        if (locked) {
            PlayerStatistics ps = PlayerStatistics.GetInstance();
            if (lockedKey == CollectableKeys.KEY_TYPE.CIRCLE && ps.hasCircleKey) {
                return true;
            }
            if (lockedKey == CollectableKeys.KEY_TYPE.SQUARE && ps.hasSquareKey) {
                return true;
            }
            if (lockedKey == CollectableKeys.KEY_TYPE.TRIANGLE && ps.hasTriangleKey) {
                return true;
            }
            return false;
        } else {
            return true;
        }
    }
}