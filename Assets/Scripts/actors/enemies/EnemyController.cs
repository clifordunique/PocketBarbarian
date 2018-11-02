using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IActorController {

    public EnemyAction defaultAction;

    public bool isShooter = false;
    [ConditionalHideAttribute("isShooter", true)]
    public GameObject projectile;
    [ConditionalHideAttribute("isShooter", true)]
    public float interval = 0.5F;
    private float lastShot;

    private AbstractEnemyState currentState;    

    [HideInInspector]
    public EnemyAction currentAction;
    private bool isInterruptAction;
    [HideInInspector]
    public IEnemyMoveController2D moveController;
    [HideInInspector]
    public AiBehaviour aiBehaviour;

    private BoxCollider2D boxCollider;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public HurtBox hurtBox;

    public void Start() {
        defaultAction.moveTarget = Vector3.positiveInfinity;
        moveController = GetComponent<IEnemyMoveController2D>();
        aiBehaviour = GetComponent<AiBehaviour>();
        animator = GetComponent<Animator>();

        if (aiBehaviour) {
            currentAction = aiBehaviour.GetCurrentAction();
        } else {
            currentAction = defaultAction;
        }

        boxCollider = GetComponent<BoxCollider2D>();

        hurtBox = GetComponent<HurtBox>();
        if (!hurtBox) {
            // search in children
            hurtBox = transform.GetComponentInChildren<HurtBox>();
        }

        // init ENEMY state
        currentState = new EnemyIdleState(this);
    }


    public void ReactHurt(bool dead, bool push, Vector3 hitSource) {

        EnemyAction hitAction = new EnemyAction(EnemyAction.ACTION_EVENT.HIT);
        if (push) {
            hitAction.hitTarget = hitSource;
        } else {
            hitAction.hitTarget = Vector3.positiveInfinity;
        }

        currentAction = hitAction;
        isInterruptAction = true;        
    }

    public void ReactHit() {
    }

    public void RequestNextAction() {
        if (aiBehaviour) {
            if (isInterruptAction) {
                // interruptAction finished
                currentAction = aiBehaviour.GetCurrentAction();
                isInterruptAction = false;
            } else {
                currentAction = aiBehaviour.GetNextAction();
            }
        } else {
            currentAction = defaultAction;
        }
    }

    private void Update() {
        // refresh AiBehaviour if not an interruptAction is finished
        if (aiBehaviour && !isInterruptAction) {
            currentAction = aiBehaviour.GetCurrentAction();
        }

        // handle StateMachine
        AbstractEnemyState newState = currentState.UpdateState();
        if (newState != null) {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }

    public void ShootProjectile(Vector3 target, bool targetIsVector) {
        if (Time.time - lastShot > interval) {
            Vector3 spawnPosition;
            if (targetIsVector) {
                spawnPosition = Utils.GetSpawnPositionProjectileVector(target, transform, boxCollider);
            } else {
                spawnPosition = Utils.GetSpawnPositionProjectileStaticTarget(target, transform, boxCollider);
            }

            GameObject projectileGo = Instantiate(projectile, spawnPosition, transform.rotation, EffectCollection.GetInstance().transform);
            projectileGo.GetComponent<Projectile>().InitProjectile(target, targetIsVector);
            lastShot = Time.time;
        }
    }
}
