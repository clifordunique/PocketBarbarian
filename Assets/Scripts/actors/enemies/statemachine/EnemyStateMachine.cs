using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

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

    public void Start() {
        defaultAction.moveTarget = Vector3.positiveInfinity;
        moveController = GetComponent<IEnemyMoveController2D>();
        aiBehaviour = GetComponent<AiBehaviour>();

        if (aiBehaviour) {
            currentAction = aiBehaviour.GetCurrentAction();
        } else {
            currentAction = defaultAction;
        }

        boxCollider = GetComponent<BoxCollider2D>();

        // init ENEMY state
        currentState = new EnemyIdleState(this);
    }

    public void InterruptAction(EnemyAction interruptAction) {
        currentAction = interruptAction;
        isInterruptAction = true;
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

            GameObject projectileGo = Instantiate(projectile, spawnPosition, transform.rotation, EffectParent.GetInstance().transform);
            projectileGo.GetComponent<Projectile>().InitProjectile(target, targetIsVector);
            lastShot = Time.time;
        }

    }
}
