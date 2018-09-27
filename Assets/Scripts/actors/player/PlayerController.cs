using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController2D))]
public class PlayerController : MonoBehaviour, IActorController {

    public LayerMask interactiveLayers;
    public GameObject prefabEffectStep;
    public GameObject prefabEffectJump;
    public GameObject prefabEffectLanding;
    public GameObject prefabEffectDashing;
    public GameObject prefabEffectDashingSilhouette;

    private AbstractState currentState;
    [HideInInspector]
    public PlayerMoveController2D moveController;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator animator;
    
    public HitBox hitBoxAttack1;
    public HitBox hitBoxDash;
    public PlayerHurtBox hurtBox;

    private float dirX = 1;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public Vector3 lastHitSource;
    [HideInInspector]
    public bool interactableInRange = false;
    [HideInInspector]
    public GameObject interactable;

    private static PlayerController _instance;

    // Use this for initialization
    void Start () {
        moveController = GetComponent<PlayerMoveController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = new IdleState(this);

        _instance = this;
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

    public void ReactHurt(bool dead, bool push, Vector3 hitSource) {
        this.dead = dead;
        lastHitSource = hitSource;
        currentState.InterruptEvent(AbstractState.ACTION.HIT);
    }

    public void ReactHit() {
        currentState.HandleEvent(AbstractState.EVENT_PARAM_HIT);
    }

    public void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();
        effectSpriteRenderer.flipX = (dirX < 0);
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
            Debug.Log("Trigger enter detected!");
            interactableInRange = true;
            interactable = collider.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (interactiveLayers == (interactiveLayers | (1 << collider.gameObject.layer))) {
            if (interactable != null && collider.gameObject == interactable) {
                Debug.Log("Trigger exit detected!");
                interactableInRange = false;
                interactable = null;
            } else {
                Debug.Log("Different exit than last enter!");
            }

        }
    }
}
