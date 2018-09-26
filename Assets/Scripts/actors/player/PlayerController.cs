using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController2D))]
public class PlayerController : MonoBehaviour, IActorController {

    public GameObject prefabEffectStep;
    public GameObject prefabEffectJump;
    public GameObject prefabEffectLanding;
    public GameObject prefabEffectDashing;

    private AbstractState currentState;
    [HideInInspector]
    public PlayerMoveController2D moveController;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator animator;
    
    public GameObject hitBoxAttack1;
    public GameObject hitBoxDash;

    private float dirX = 1;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public Vector3 lastHitSource;

    // Use this for initialization
    void Start () {
        moveController = GetComponent<PlayerMoveController2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentState = new IdleState(this);
    }

    public void updateSpriteDirection(float dirX) {
        if (this.dirX != dirX && dirX != 0) {
            this.dirX = dirX;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }            
    }

    public void HandleAnimEvent(string parameter) {
        currentState.HandleAnimEvent(parameter);
    }



    public void ReactHurt(bool dead, bool push, Vector3 hitSource) {
        this.dead = dead;
        lastHitSource = hitSource;
        currentState.InterruptEvent(AbstractState.ACTION.HIT);
    }

    public void ReactHit() {
        currentState.InterruptEvent(AbstractState.ACTION.IDLE);
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
}
