using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IActorController {

    public EnemyAction defaultAction;
    public float facingDirectionX = 1;
    public float randomDelaySeconds = 0;
    private float delayTime = 0;

    public bool isShooter = false;
    [ConditionalHideAttribute("isShooter", true)]
    public GameObject projectile;
    [ConditionalHideAttribute("isShooter", true)]
    public float interval = 0.5F;
    private float lastShot;

    private AbstractEnemyState currentState;    

    [HideInInspector]
    public EnemyAction currentAction;
    [HideInInspector]
    public bool isInterruptAction;
    [HideInInspector]
    public IEnemyMoveController2D moveController;
    [HideInInspector]
    public AiBehaviour aiBehaviour;

    private BoxCollider2D boxCollider;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public HurtBox hurtBox;
    [HideInInspector]
    public HitBox.DAMAGE_TYPE lastDamageType;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public void Start() {
        defaultAction.moveTarget = Vector3.positiveInfinity;
        moveController = GetComponent<IEnemyMoveController2D>();
        aiBehaviour = GetComponent<AiBehaviour>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetDirection(facingDirectionX);

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

        if (randomDelaySeconds > 0) {
            delayTime = Random.Range(0F, randomDelaySeconds);
            animator.speed = 0;
        }
    }


    public void ReactHurt(bool dead, bool push, bool instakill, Vector3 hitSource, HitBox.DAMAGE_TYPE damageType) {

        EnemyAction hitAction = new EnemyAction(EnemyAction.ACTION_EVENT.HIT);
        if (push && !instakill) {
            hitAction.hitTarget = hitSource;
        } else {
            hitAction.hitTarget = Vector3.positiveInfinity;
        }
        lastDamageType = damageType;
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
        if (delayTime > 0) {
            // check if delayTime reached
            if (Time.timeSinceLevelLoad > delayTime) {
                delayTime = 0;
                animator.speed = 1;
            }
        } else {

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
    }

    public void ShootProjectile(Vector3 target, bool targetIsVector) {
        if (Time.timeSinceLevelLoad - lastShot > interval) {
            Vector3 spawnPosition;
            if (targetIsVector) {
                spawnPosition = Utils.GetSpawnPositionProjectileVector(target, transform, boxCollider);
            } else {
                spawnPosition = Utils.GetSpawnPositionProjectileStaticTarget(target, transform, boxCollider);
            }

            GameObject projectileGo = Instantiate(projectile, spawnPosition, transform.rotation, EffectCollection.GetInstance().transform);
            projectileGo.GetComponent<Projectile>().InitProjectile(target, targetIsVector);
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    public void SetDirection(float dirX) {
        facingDirectionX = dirX;
        if (dirX < 0) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }
}
