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

    public GameObject prefabSquishEffect;
    

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

            MovePassengers(true, velocity);
            transform.Translate(velocity);
            MovePassengers(false, velocity);
        }
    }


    public abstract Vector3 CalculatePlatformMovement();


    void MovePassengers(bool beforeMovePlatform, Vector3 velocity) {
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
                        passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
                    } else {
                        if (passenger.velocity.y > 0 && passenger.standingOnPlatform && passengerDictionary[passenger.transform].IsAbove()) {
                        } else {
                            passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
                        }
                    }

                    bool squisch = false;
                    Vector3 hitSourcePosition = transform.position;

                    if ((!passenger.standingOnPlatform && velocity.x < 0 && passengerDictionary[passenger.transform].IsLeft()) ||
                        (!passenger.standingOnPlatform && velocity.x > 0 && passengerDictionary[passenger.transform].IsRight())) {
                        // sideways squisch!
                        squisch = true;
                        hitSourcePosition = new Vector3(transform.position.x, passenger.transform.position.y, transform.position.z);
                    }

                    if ((passenger.standingOnPlatform && velocity.y > 0 && passengerDictionary[passenger.transform].IsBelow() && passengerDictionary[passenger.transform].IsAbove()) ||
                        (!passenger.standingOnPlatform && passengerDictionary[passenger.transform].IsBelow() && velocity.y < 0)) {
                        // vertical squisch!
                        squisch = true;
                        hitSourcePosition = new Vector3(passenger.transform.position.x, transform.position.y, transform.position.z);
                    }

                    if (squisch) {                        
                        HurtBox hurtBox = passenger.transform.GetComponent<HurtBox>();
                        if (!hurtBox) {
                            hurtBox = passenger.transform.GetComponentInChildren<HurtBox>();
                        }

                        if (hurtBox) {
                            hurtBox.ReceiveHit(true, 100, HitBox.DAMAGE_TYPE.SQUISH, hitSourcePosition);

                            // create squish effect on platform
                            Vector3 positionEffectPlatform = BoundUtils.GetPositionOnBounds(velocity, passenger.transform, myCollider.bounds, 26);
                            InstantiateEffect(prefabSquishEffect, positionEffectPlatform, BoundUtils.GetEffectRotation(velocity, true), transform);

                            // create squish effect on ground
                            BoxCollider2D collider2D = passenger.transform.GetComponent<BoxCollider2D>();
                            if (collider2D) {
                                Vector3 positionEffectGround = BoundUtils.GetPositionOnBounds(velocity, passenger.transform, collider2D.bounds);
                                InstantiateEffect(prefabSquishEffect, positionEffectGround, BoundUtils.GetEffectRotation(velocity, false));
                            }

                        }
                    }
                }
            }
        }
    }

    public void InstantiateEffect(GameObject effectToInstanciate, Vector2 position, float rotateAngel = 0F, Transform parent = null) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        if (parent == null) {
            effect.transform.parent = EffectCollection.GetInstance().transform;
        } else {
            effect.transform.parent = parent;
        }
        effect.transform.position = position;
        if (rotateAngel != 0) {
            effect.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotateAngel));
        }
    }

    private float GetEffectRotation(Vector3 velocity, bool reverse) {
        float angel = 0;
        if (velocity.x > 0) {
            angel = (reverse ? 90f : -90f);
        }
        if (velocity.x < 0) {
            angel = (reverse ? -90f : 90f);
        }
        if (velocity.y < 0) {
            angel = (reverse ? 180f : 0f);
        }
        if (velocity.y > 0) {
            angel = (reverse ? 0f : 180f);
        }
        return angel;
    }

    private Vector2 GetEffectSquishPositionGround(Vector3 velocity, Transform passenger) {

        Vector2 effectPosition = Vector3.zero;
        BoxCollider2D collider2D = passenger.GetComponent<BoxCollider2D>();
        if (!collider2D) {
            return Vector2.zero;
        }
        Bounds bounds = collider2D.bounds;
        if (velocity.x > 0) {
            float newX = bounds.center.x + bounds.extents.x;
            float newY = passenger.position.y;
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.x < 0) {
            float newX = bounds.center.x - bounds.extents.x;
            float newY = passenger.position.y;
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.y > 0) {
            float newY = bounds.center.y + bounds.extents.y;
            float newX = passenger.position.x;
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.y < 0) {
            float newY = bounds.center.y - bounds.extents.y;
            float newX = passenger.position.x;
            effectPosition = new Vector2(newX, newY);
        }
        return effectPosition;
    }

    private Vector2 GetEffectSquishPositionPlatform(Vector3 velocity, Transform passenger) {

        Vector2 effectPosition = Vector3.zero;
        Bounds bounds = myCollider.bounds;
        if (velocity.x > 0) {
            float newX = bounds.center.x + bounds.extents.x;
            float newY = CalculateEffectPositionY(passenger.position);
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.x < 0) {
            float newX = bounds.center.x - bounds.extents.x;
            float newY = CalculateEffectPositionY(passenger.position);
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.y > 0) {
            float newY = bounds.center.y + bounds.extents.y;
            float newX = CalculateEffectPositionX(passenger.position);
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.y < 0) {
            float newY = bounds.center.y - bounds.extents.y;
            float newX = CalculateEffectPositionX(passenger.position);
            effectPosition = new Vector2(newX, newY);
        }
        return effectPosition;        
    }

    private float CalculateEffectPositionX(Vector3 passenger) {
        Bounds bounds = myCollider.bounds;
        float distanceEffectX = Utils.PixelToWorldunits(26) / 2;
        float newX = passenger.x;
        
        if (bounds.min.x > (passenger.x - distanceEffectX)) {
            newX = bounds.min.x + distanceEffectX;
        }
        if (bounds.max.x < (passenger.x + distanceEffectX)) {
            newX = bounds.max.x - distanceEffectX;
        }
        return newX;
    }

    private float CalculateEffectPositionY(Vector3 passenger) {
        Bounds bounds = myCollider.bounds;
        float distanceEffectY = Utils.PixelToWorldunits(26) / 2;
        float newY = passenger.y;
        
        if (bounds.min.x > (passenger.x - distanceEffectY)) {
            newY = bounds.min.y + distanceEffectY;
        }
        if (bounds.max.x < (passenger.x + distanceEffectY)) {
            newY = bounds.max.y - distanceEffectY;
        }
        return newY;
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

