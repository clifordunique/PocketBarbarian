using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public float timeDoubleDash;

    private float timeLastDirectionX = -1;
    private float lastDirectionX;
    private bool isDashing;

    private static InputController _instance;

    public void Awake() {
        _instance = this;
    }

    public static InputController GetInstance() {
        return _instance;
    }

    public bool IsDashing() {
        return isDashing;
    }

    public bool IsJumpKeyDown() {
        return (Input.GetKeyDown(KeyCode.Space));
    }

    public bool IsAttack1KeyDown() {
        return (Input.GetKeyDown(KeyCode.X));
    }

    public bool IsJumpKeyUp() {
        return (Input.GetKeyUp(KeyCode.Space));
    }

    public float GetDirectionX() {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetDirectionY() {
        return Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void Update () {
		if (((Input.GetKeyDown(KeyCode.LeftArrow) && lastDirectionX == -1) || (Input.GetKeyDown(KeyCode.RightArrow) && lastDirectionX == 1))) {
            // double direction, check for time between
            if (timeLastDirectionX != -1 && Time.time - timeLastDirectionX <= timeDoubleDash) {
                isDashing = true;
                return;
            }
            timeLastDirectionX = Time.time;
        }
        isDashing = false;        
        if (GetDirectionX() != 0) {
            lastDirectionX = GetDirectionX();
            timeLastDirectionX = Time.time;
        }
        
    }
}
