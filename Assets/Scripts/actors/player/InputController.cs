using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public float timeDoubleDash;
    public bool moveInputEnabled = true;
    public float timeLookAroundActivate = 0F;

    private float timeLastDirectionX = -1;
    private float lastDirectionX;
    private bool isDashing;

    private float timeLastUp = -1;
    private bool isWarcry;

    private float timeLookAroundStart = -1;
    private float lastDirectionY;
    private bool lookAroundEnabled = false;

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
        if (moveInputEnabled) {
            return (Input.GetKeyDown(KeyCode.Space));
        }
        return false;
    }

    public bool IsAttack1KeyPressed() {
        if (moveInputEnabled) {
            return (Input.GetKey(KeyCode.X));
        }
        return false;
    }

    public bool IsAttack1KeyDown() {
        if (moveInputEnabled) {
            return (Input.GetKeyDown(KeyCode.X));
        }
        return false;
    }

    public bool IsAttack2KeyPressed() {
        if (moveInputEnabled) {
            return (Input.GetKey(KeyCode.C));
        }
        return false;
    }

    public bool IsAttack2KeyDown() {
        if (moveInputEnabled) {
            return (Input.GetKeyDown(KeyCode.C));
        }
        return false;
    }

    public bool IsJumpKeyUp() {
        if (moveInputEnabled) {
            return (Input.GetKeyUp(KeyCode.Space));
        }
        return false;
    }

    public bool DownKeyDown() {
        if (moveInputEnabled) {
            return (Input.GetKey(KeyCode.DownArrow));
        }
        return false;
    }


    public float GetDirectionX() {
        if (moveInputEnabled) {
            return Input.GetAxisRaw("Horizontal");
        }
        return 0F;
    }

    public float GetDirectionY() {
        if (moveInputEnabled) {
            return Input.GetAxisRaw("Vertical");
        }
        return 0F;
    }

    public float GetDirectionLookaround() {
        if (moveInputEnabled && lookAroundEnabled) {
            return Input.GetAxisRaw("Vertical");
        }
        return 0F;
    }

    public bool IsSwitchPotionsKeyDown() {
        if (moveInputEnabled) {
            return (Input.GetKeyDown(KeyCode.Alpha1));
        }
        return false;
    }

    public bool IsUsePotionsKeyDown() {
        if (moveInputEnabled) {
            return (Input.GetKeyDown(KeyCode.V));
        }
        return false;
    }

    public bool IsWarcry() {
        if (moveInputEnabled) {
            return isWarcry;
        }
        return false;
    }


    public bool AnyKeyDown() {
        return Input.anyKeyDown;
    }

    // Update is called once per frame
    void LateUpdate () {
		if (((Input.GetKeyDown(KeyCode.LeftArrow) && lastDirectionX == -1) || (Input.GetKeyDown(KeyCode.RightArrow) && lastDirectionX == 1))) {
            // double direction, check for time between
            if (timeLastDirectionX != -1 && Time.timeSinceLevelLoad - timeLastDirectionX <= timeDoubleDash) {
                isDashing = true;
                return;
            }
            timeLastDirectionX = Time.timeSinceLevelLoad;
        }
        isDashing = false;        
        if (GetDirectionX() != 0) {
            lastDirectionX = GetDirectionX();
            timeLastDirectionX = Time.timeSinceLevelLoad;
        }

        CheckLookAhead();

        CheckWarcry();
    }

    private void CheckLookAhead() {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) {
            // start counting time
            lastDirectionY = Input.GetAxisRaw("Vertical");
            timeLookAroundStart = Time.timeSinceLevelLoad;
        }
        if (lastDirectionY != 0 && Input.GetAxisRaw("Vertical") == lastDirectionY && PlayerController.GetInstance().moveController.IsGrounded()) {
            if (timeLookAroundStart + timeLookAroundActivate < Time.timeSinceLevelLoad) {
                lookAroundEnabled = true;
            }
        } else {
            lastDirectionY = 0F;
            lookAroundEnabled = false;
        }
    }

    private void CheckWarcry() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (timeLastUp + timeDoubleDash > Time.timeSinceLevelLoad) {
                // vor kurzem up betätigt, also Warcry
                Debug.Log("Is Warcry!!!");
                isWarcry = true;
                return;
            }
            timeLastUp = Time.timeSinceLevelLoad;
        }
        isWarcry = false;
    }
}
