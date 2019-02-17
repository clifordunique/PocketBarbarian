using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController2D))]
public class PlayerController: MonoBehaviour, IActorController {



    public LayerMask interactiveLayers;

    [Header("Weapon Settings")]
    public bool hasWeapon = false;
    public bool comboAllowed = false;
    public List<string> weaponUuid;
    public List<int> weaponLayerId;
    public HitBox[] weaponHitBoxList;
    public HitBox hitBoxDash;


    [Header("Projectile Settings")]
    public GameObject prefabProjectile;
    public GameObject spawnPositionProjectile;


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
    public GameObject prefabEffectGroundHitOneSided;
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
    public LastHit lastHit = new LastHit();
    [HideInInspector]
    public bool interactableInRange = false;
    [HideInInspector]
    public GameObject interactable;
    [HideInInspector]
    public PlayerStatistics statistics;
    [HideInInspector]
    public InputController input;

    private AbstractState currentState;
    private static PlayerController _instance;

    // Use this for initialization
    void Awake() {
        moveController = GetComponent<PlayerMoveController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = new SwordUpState(this);
        statistics = GetComponentInChildren<PlayerStatistics>();
        input = GetComponent<InputController>();
        hurtBox = GetComponentInChildren<PlayerHurtBox>();
        _instance = this;

        // init weapon and animation layer
        SetWeapon("-1", 0);
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

    public void InterruptState(AbstractState.ACTION action) {
        if (currentState != null) {
            currentState.InterruptEvent(action);
        }
    }

    public void ReactHurt(bool dead, bool push, bool instakill, Vector3 hitSource, HitBox.DAMAGE_TYPE damageType) {
        lastHit.push = push;
        lastHit.instakill = instakill;
        lastHit.hitSource = hitSource;
        lastHit.damageType = damageType;
        InterruptState(AbstractState.ACTION.HIT);
    }

    public void ReactHit() {
        currentState.HandleEvent(AbstractState.EVENT_PARAM_HIT);
    }

    public void InstantiateEffect(GameObject effectToInstanciate, float offsetX = 0F) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();
        if (effectSpriteRenderer) {
            effectSpriteRenderer.flipX = (dirX < 0);
        }
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = new Vector2(transform.position.x + offsetX, transform.position.y);
    }

    // Update is called once per frame
    void Update() {

        if (Time.timeScale != 0) {
            // handle StateMachine
            AbstractState newState = currentState.UpdateState();
            if (newState != null) {
                currentState.OnExit();
                currentState = newState;
                currentState.OnEnter();
            }
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
        input.moveInputEnabled = inputEnabled;
        spriteRenderer.enabled = showPlayer;
    }


    public void SetWeapon(string uuid, int damage) {
        hasWeapon = true;
        if (uuid == "-1") {
            // no weapon
            hasWeapon = false;
        }

        int index = weaponUuid.IndexOf(uuid);
        int layer = weaponLayerId[index];
        if (layer >= 0 && layer < animator.layerCount) {
            SetAnimatorLayer(layer);
        } else {
            Debug.LogError("Weapon Animator layer " + layer + " not supported!");
        }

        foreach (HitBox hitbox in weaponHitBoxList) {
            hitbox.damage = damage;
        }
    }

    private void SetAnimatorLayer(int activeLayer) {
        for (int i = 0; i < animator.layerCount; i++) {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(activeLayer, 1);
    }

    public struct LastHit {
        public Vector3 hitSource;
        public HitBox.DAMAGE_TYPE damageType;
        public bool push;
        public bool instakill;
    }
}
