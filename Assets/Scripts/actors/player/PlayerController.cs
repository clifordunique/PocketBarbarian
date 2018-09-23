using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject prefabEffectStep;
    public GameObject prefabEffectJump;
    public GameObject prefabEffectLanding;

    private AbstractState currentState;
    [HideInInspector]
    public MoveGroundController2D moveController;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Animator animator;

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

    public void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject dustGo = (GameObject)Instantiate(effectToInstanciate);
        SpriteRenderer effectSpriteRenderer = dustGo.GetComponent<SpriteRenderer>();
        effectSpriteRenderer.flipX = (dirX < 0);
        dustGo.transform.parent = EffectParent.GetInstance().transform;
        dustGo.transform.position = transform.position;

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
