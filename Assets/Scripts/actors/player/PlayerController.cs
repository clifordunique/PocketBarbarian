using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController2D))]
public class PlayerController : MonoBehaviour, IActorController {



    public LayerMask interactiveLayers;

    public bool hasWeapons = false;

    [Header("Projectile Settings")]
    public GameObject prefabProjectile;
    public GameObject spawnPositionProjectile;

    [Header("HitBoxes")]
    public HitBox hitBoxAttack1;
    public HitBox hitBoxDash;

    [Header("Effect Prefabs")]
    public GameObject prefabEffectStep;
    public GameObject prefabEffectJump;
    public GameObject prefabEffectLanding;
    public GameObject prefabEffectDashing;
    public GameObject prefabEffectDashingSilhouette;
    public GameObject prefabEffectDashingHit;
    public GameObject prefabEffectStompingSilhouette;
    public GameObject prefabEffectStompingGround;
    public GameObject prefabEffectFallingSilhouette;
    public GameObject prefabEffectHardLanding;
    public GameObject prefabEffectWallJump;
    public ParticleSystem sparkParticle;

    [Header("Outline Effect Material")]
    public Material outlineMaterial;

    [HideInInspector]
    public PlayerMoveController2D moveController;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerHurtBox hurtBox;
    [HideInInspector]
    public float dirX = 1;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public Vector3 lastHitSource;
    [HideInInspector]
    public bool interactableInRange = false;
    [HideInInspector]
    public GameObject interactable;
    [HideInInspector]
    public PlayerStatistics statistics;

    private AbstractState currentState;
    private static PlayerController _instance;

    // Use this for initialization
    void Awake () {
        moveController = GetComponent<PlayerMoveController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = new IdleState(this);
        statistics = GetComponentInChildren<PlayerStatistics>();
        hurtBox = GetComponentInChildren<PlayerHurtBox>();
        _instance = this;

        if (hasWeapons) {
            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0);
            moveController.wallJumpingAllowed = true;
        } else {
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
            moveController.wallJumpingAllowed = false;
        }
    }

    public static PlayerController GetInstance() {
        return _instance;
    }


    public void updateSpriteDirection(float dirX) {
        if (this.dirX != dirX && dirX != 0) {
            this.dirX = dirX;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }            
    }

    public void HandleEvent(string parameter) {
        currentState.HandleEvent(parameter);
    }

    public void ReactHurt(bool dead, bool push, Vector3 hitSource, HitBox.DAMAGE_TYPE damageType) {
        this.dead = dead;
        lastHitSource = hitSource;
        if (damageType == HitBox.DAMAGE_TYPE.WATER) {
            currentState.InterruptEvent(AbstractState.ACTION.DEATH);
        } else {
            currentState.InterruptEvent(AbstractState.ACTION.HIT);
        }
    }

    public void ReactHit() {
        currentState.HandleEvent(AbstractState.EVENT_PARAM_HIT);
    }

    public void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();
        if (effectSpriteRenderer) {
            effectSpriteRenderer.flipX = (dirX < 0);
        }
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update () {

        // handle StateMachine
        AbstractState newState = currentState.UpdateState();
        if (newState != null) {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (interactiveLayers == (interactiveLayers | (1 << collider.gameObject.layer))) {
            interactableInRange = true;
            interactable = collider.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (interactiveLayers == (interactiveLayers | (1 << collider.gameObject.layer))) {
            if (interactable != null && collider.gameObject == interactable) {
                interactableInRange = false;
                interactable = null;
            } else {
                Debug.Log("Different exit than last enter!");
            }

        }
    }

    public void FlashOutline() {
        SpriteOutlineEffect so = new SpriteOutlineEffect(spriteRenderer.material, outlineMaterial);
        StartCoroutine(so.OutlineFlashing(spriteRenderer, 0.75F));
    }

    public void ShootProjectile() {
        Vector3 target = new Vector3(dirX, 0, 0);

        GameObject projectileGo = Instantiate(prefabProjectile, spawnPositionProjectile.transform.position, transform.rotation, EffectCollection.GetInstance().transform);
        projectileGo.GetComponent<Projectile>().InitProjectile(target, true);        
    }

    public void SetEnabled(bool inputEnabled, bool showPlayer) {
        InputController.GetInstance().moveInputEnabled = inputEnabled;
        spriteRenderer.enabled = showPlayer;
    }
}
