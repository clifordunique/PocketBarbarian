using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController: MonoBehaviour, IActorController {

    public EnemyAction defaultAction;
    public float randomDelaySeconds = 0;
    private float delayTime = 0;


    public bool isShooter = false;
    [ConditionalHideAttribute("isShooter", true)]
    public Transform spawnPosition;
    [ConditionalHideAttribute("isShooter", true)]
    public GameObject projectile;
    [ConditionalHideAttribute("isShooter", true)]
    public float interval = 0.5F;
    [HideInInspector]
    public float lastShot;

    public GameObject dizzyEffect;
    public GameObject moveEffectPrefab;

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
    public HitBox hitBox;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public virtual void Start() {
        defaultAction.moveTarget = Vector3.positiveInfinity;
        moveController = GetComponent<IEnemyMoveController2D>();
        aiBehaviour = GetComponent<AiBehaviour>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitBox = GetComponentInChildren<HitBox>();

        if (aiBehaviour && aiBehaviour.enabled) {

            aiBehaviour.defaultAction = defaultAction;
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

        // look up if hit animation present
        if (Utils.HasParameter("HIT", animator)) {
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
    }

    public void ReactDizzy(float seconds) {
        EnemyAction dizzyAction = new EnemyAction(EnemyAction.ACTION_EVENT.DIZZY);
        dizzyAction.amount = seconds;
        currentAction = dizzyAction;
        isInterruptAction = true;
    }

    public void ReactHit() {
    }

    public void RequestNextAction() {
        if (aiBehaviour && aiBehaviour.enabled) {
            if (isInterruptAction) {
                // interruptAction finished
                currentAction = aiBehaviour.GetCurrentAction();
            } else {
                currentAction = aiBehaviour.GetNextAction();
            }
        } else {
            currentAction = defaultAction;
        }
        isInterruptAction = false;
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
            if (aiBehaviour && aiBehaviour.enabled && !isInterruptAction) {
                currentAction = aiBehaviour.GetCurrentAction();
            }

            CheckForFalling();

            // handle StateMachine
            AbstractEnemyState newState = currentState.UpdateState();
            if (newState != null) {
                currentState.OnExit();
                currentState = newState;
                currentState.OnEnter();
            }
        }
    }

    private void CheckForFalling() {
        // if falling and current action is no interruptAction
        if (moveController.IsFalling() && !isInterruptAction) {
            Debug.Log("EnemyIsFalling");
            EnemyAction fallingAction = new EnemyAction(EnemyAction.ACTION_EVENT.FALLING);
            currentAction = fallingAction;
        }
    }

    public virtual void ShootProjectile(Vector3 target, bool targetIsVector) {
        if (Time.timeSinceLevelLoad - lastShot > interval) {
            Vector3 currentSpawnPosition;
            if (spawnPosition != null) {
                currentSpawnPosition = spawnPosition.position;
            } else {
                if (targetIsVector) {
                    currentSpawnPosition = Utils.GetSpawnPositionProjectileVector(target, transform, boxCollider);
                } else {
                    currentSpawnPosition = Utils.GetSpawnPositionProjectileStaticTarget(target, transform, boxCollider);
                }
            }

            GameObject projectileGo = Instantiate(projectile, currentSpawnPosition, transform.rotation, EffectCollection.GetInstance().transform);
            projectileGo.GetComponent<Projectile>().InitProjectile(target, targetIsVector);
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    public void SetDirection(float dirX) {
        if (transform.localScale.x != dirX && dirX != 0) {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void HandleAnimEvent(string parameter) {
        currentState.HandleAnimEvent(parameter);
    }

    public void InstantiateEffect(GameObject effectToInstanciate, Vector2 position, float rotateAngel = 0F) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);

        float scaleX = transform.localScale.x;
        effect.transform.localScale = new Vector3(scaleX, effect.transform.localScale.y, effect.transform.localScale.z);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = position;
        if (rotateAngel != 0) {
            effect.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotateAngel));
        }
    }
}
