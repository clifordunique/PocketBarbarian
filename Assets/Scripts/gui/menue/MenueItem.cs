using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItem : MonoBehaviour
{
    public AbstractMenueManager.MENUEITEM_TYPE menueItemType;

    private IMenueItemSprite sr;
    private SimpleMovementPosition movement;

    [HideInInspector]
    public bool movementComplete = false;

    public void Awake() {
        movement = GetComponent<SimpleMovementPosition>();
        sr = transform.GetComponentInChildren<IMenueItemSprite>();
    }
    void OnMouseEnter() {
        if (AbstractMenueManager.GetInstance().menueInputEnabled) {
            AbstractMenueManager.GetInstance().MenueItemFocused(this);
        }
    }

    void OnMouseExit() {
        //Deselect();
    }

    void OnMouseDown() {
        if (AbstractMenueManager.GetInstance().menueInputEnabled) {
            Click();
        }
    }

    public void Select() {
        if (Cursor.visible) {
            sr.SetEnabled();            
        }
    }

    public void Deselect() {
        sr.SetDisabled();
    }

    public void Click() {
        sr.Click();
        AbstractMenueManager.GetInstance().MenueItemSelected(menueItemType);
    }

    public void ShowItem() {
        movement.StartMoving();
        StartCoroutine(CheckIfMovementComplete());
    }

    IEnumerator CheckIfMovementComplete() {
        while (!movement.EndPosReached()) {
            yield return new WaitForEndOfFrame();
        }
        // fertig
        Debug.Log("Movement Complete!");
        movementComplete = true;
    }

    public float GetWidth() {
        return sr.GetWidth();
    }
}
