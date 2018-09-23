using UnityEngine;
using System.Collections;


public class MoveGroundController2D: MoveController2D {

    [Header("In Air Settings")]
    public bool jumpingAllowed = false;
    [ConditionalHide("jumpingAllowed", true)]
    public float maxJumpHeight = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public float minJumpHeight = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public float timeToJumpApex = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public bool doubleJumpAllowed = false;
    [ConditionalHide("jumpingAllowed", true)]
    public float accelerationTimeAirborne = 0F;//.2f;

    public float comicFallFactor = 1.08f;
    public float stampingFallFactor = 2f;

    [Header("On Ground Settings")]
    public float moveSpeed = 6;
    public float accelerationTimeGrounded = 0F;//.05f;

    public bool dashAllowed = false;
    [ConditionalHide("dashAllowed", true)]
    public float moveSpeedDash = 12;
    [ConditionalHide("dashAllowed", true)]
    public float dashDuration = 0.5F;

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public float targetVelocityX;

    float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;	
    
    float velocityXSmoothing;
    bool isStamping = false;
    bool isPushed = false;
    float endTimePush;
    int jumpCounter = 0;


    public override void Start() {
        base.Start();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	public virtual void Update() {
		CalculateVelocity ();
        Move (velocity * Time.deltaTime);

		if (collisions.above || collisions.below) {			
			velocity.y = 0;
            isStamping = false;
            jumpCounter = 0;
        }
        
        if (isPushed && endTimePush < Time.time) {
            isPushed = false;            
        }
    }

	public void OnMove (float moveDirectionX, float moveDirectionY) {		
        if (!isPushed) {
            this.moveDirectionX = moveDirectionX;
            this.moveDirectionY = moveDirectionY;
            targetVelocityX = moveDirectionX * moveSpeed;
        }
    }

	public void OnJumpInputDown() {
        if (collisions.below || (doubleJumpAllowed && jumpCounter == 1)) {
            velocity.y = maxJumpVelocity;
            jumpCounter++;
        }
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

    public void OnStamp() {
        if (!collisions.below) {
            isStamping = true;
            velocity.y = 0;
        }
    }

    public void OnDash() {
        if (collisions.below) {
            isPushed = true;
            targetVelocityX = moveDirectionX * moveSpeedDash;
            endTimePush = Time.time + dashDuration;
        }
    }

    public void OnPush(float pushDirectionX, float pushDirectionY, float pushForce, float pushDuration) {
        isPushed = true;
        targetVelocityX = pushDirectionX * pushForce;
        //if (pushDirectionY > 0) {
            velocity.y = minJumpVelocity;
        //}
        endTimePush = Time.time + pushDuration;
    }

    public bool IsFalling () {
        return (!collisions.below && velocity.y <= 0);
    }

    public bool IsGrounded() {
        return (collisions.below && velocity.y <= 0);
    }

    void CalculateVelocity() {

        
        if (IsFalling()) {
            // is falling, comic Fall Factor
            velocity.y += gravity * Time.deltaTime * comicFallFactor;
        } else {
            velocity.y += gravity * Time.deltaTime;
        }
        

        if (isStamping && IsFalling() && !isPushed) {
            // if stamping, no movement!
            velocity.x = 0;
            velocity.y *= stampingFallFactor;
        } else {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


        }
	}
}
