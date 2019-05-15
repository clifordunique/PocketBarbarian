using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCollisionStateChanger : MonoBehaviour
{
    // layers to react on contact
    public LayerMask contactLayers;
    public bool startActive = false;
    public bool activateOnCollision = true;
    public float delay = 0F;
    public float changeBackTime = 0f;

    private Animator anim;
    private bool active = false;
    private bool changeInAction = false;

    // Start is called before the first frame update
    void Start()
    {
        active = startActive;
        anim = GetComponent<Animator>();
    }



    public void OnCollisionEnter2D(Collision2D collision) {
        // react to hit
        if ((contactLayers == (contactLayers | (1 << collision.gameObject.layer))) && !changeInAction) {
            if ((activateOnCollision && !active) || (!activateOnCollision && active)) {
                changeInAction = true;
                StartCoroutine(ChangeStateAfterTime(delay));
            }
        }
    }


    public void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("CollisionExit");
    }

    IEnumerator ChangeStateAfterTime(float time) {
        yield return new WaitForSeconds(time);

        ChangeState(activateOnCollision);
        changeInAction = false;
        StartCoroutine(ChangeStateBackAfterTime(changeBackTime));        
    }


    IEnumerator ChangeStateBackAfterTime(float time) {
        yield return new WaitForSeconds(time);

        ChangeState(!activateOnCollision);
    }

    private void ChangeState(bool activate) {
        this.active = activate;
        if (active) {
            anim.SetBool("STATE_ACTIVE", true);
            anim.SetBool("STATE_PASSIVE", false);
        } else {
            anim.SetBool("STATE_ACTIVE", false);
            anim.SetBool("STATE_PASSIVE", true);
        }
    }
    
}
