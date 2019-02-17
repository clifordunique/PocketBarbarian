using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPlatformController2D: RaycastController2D, ITriggerReactor {

    [Header("Trigger Settings")]
    public bool useTrigger = false;
    [ConditionalHide("useTrigger", true)]
    public bool triggerActivated = false;
    [ConditionalHide("useTrigger", true)]
    public bool reactOnTriggerActivate = true;
    [ConditionalHide("useTrigger", true)]
    public bool reactOnTriggerDeactivate = true;
    [ConditionalHide("useTrigger", true)]
    public bool reactToTriggerOnce = true;

    public LayerMask passengerMask;
    public bool movePixelPerfect;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, IMove> passengerDictionary = new Dictionary<Transform, IMove>();

    public override void Start() {
        base.Start();
    }

    void Update() {
        if (!useTrigger || (useTrigger && triggerActivated)) {
            UpdateRaycastOrigins();

            Vector3 velocity = CalculatePlatformMovement();

            if (movePixelPerfect) {
                velocity = Utils.MakePixelPerfect(velocity);
            }

            CalculatePassengerMovement(velocity);

            MovePassengers(true);
            transform.Translate(velocity);
            MovePassengers(false);
        }
    }


    public abstract Vector3 CalculatePlatformMovement();


    void MovePassengers(bool beforeMovePlatform) {
        foreach (PassengerMovement passenger in passengerMovement) {

            if (!passengerDictionary.ContainsKey(passenger.transform)) {
                if (passenger.transform.GetComponent<IMove>() != null) {
                    passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<IMove>());
                }
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform) {
                if (passengerDictionary.ContainsKey(passenger.transform)) {
                    CameraFollow.GetInstance().CheckForPlayerOnPlatform(passenger.transform);

                    if (passenger.velocity.y < 0 && !passenger.standingOnPlatform && passengerDictionary[passenger.transform].IsBelow()) {
                    } else {
                        if (passenger.velocity.y > 0 && passenger.standingOnPlatform && passengerDictionary[passenger.transform].IsAbove()) {
                        } else {
                            passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
                        }
                    }                    
                }
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0) {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.blue);
                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0) {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;                        
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
                Debug.DrawRay(rayOrigin, Vector2.up, Color.red);
                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;
                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    public bool TriggerActivated() {
        if (reactOnTriggerActivate) {
            triggerActivated = true;
            return true;
        }
        return false;
    }

    public bool TriggerDeactivated() {
        if (reactOnTriggerDeactivate) {
            triggerActivated = false;
            return true;
        }
        return false;
    }

    struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform) {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

}

