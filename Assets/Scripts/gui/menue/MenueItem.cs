using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItem : MonoBehaviour
{
    private IMenueItemSprite sr;
    private SimpleMovementPosition movement;

    [HideInInspector]
    public bool movementComplete = false;

    public void Awake() {
        movement = GetComponent<SimpleMovementPosition>();
        sr = transform.GetComponentInChildren<IMenueItemSprite>();
    }
    void OnMouseEnter() {
        AbstractMenueManager.GetInstance().MenueItemSelected(this);
    }

    void OnMouseExit() {
        //Deselect();
    }

    void OnMouseDown() {
        Click();
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
