using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject prefabEffectStep;
    public GameObject prefabEffectJump;
    public GameObject prefabEffectLanding;
    public GameObject prefabEffectDashing;

    private AbstractState currentState;
    [HideInInspector]
    public MoveGroundController2D moveController;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator animator;
    
    public GameObject hitBoxAttack1;
    public GameObject hitBoxDash;

    private float dirX = 1;

    // Use this for initialization
    void Start () {
        moveController = GetComponent<MoveGroundController2D>();
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

    public void InterruptEvent(string parameter) {
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
