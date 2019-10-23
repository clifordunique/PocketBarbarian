using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMovementWaypoints: MonoBehaviour{
    
 
	public Transform[] localWaypoints;
	Vector3[] globalWaypoints;

	public float speed;
	public bool cyclic;
	public float waitTime;
	[Range(0,2)]
	public float easeAmount;

    public bool useWithAnimator = false;
    public float startDelayTime = 0F;
    public string animatorStartParameter;
    public string animatorStopParameter;

	int fromWaypointIndex;
	float percentBetweenWaypoints;
    [HideInInspector]
	public float nextMoveTime;

    private bool moving = false;
    private Animator anim;
	
	public void Start () {
        if (useWithAnimator) {
            anim = GetComponent<Animator>();
        }

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i =0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i].position;
		}
    }
    

	float Ease(float x) {
		float a = easeAmount + 1;
		return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a));
	}
	
	public void Update() {

        if ((Time.timeSinceLevelLoad + startDelayTime > nextMoveTime) && useWithAnimator && !moving) {
            // los gehts
            anim.SetBool(animatorStartParameter, true);
            anim.SetBool(animatorStopParameter, false);
            moving = true;
        }

        if (Time.timeSinceLevelLoad > nextMoveTime) {

            fromWaypointIndex %= globalWaypoints.Length;
            int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
            float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
            percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
            percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
            float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

            Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPos);

            if (percentBetweenWaypoints >= 1) {
                percentBetweenWaypoints = 0;
                fromWaypointIndex++;

                if (!cyclic) {
                    if (fromWaypointIndex >= globalWaypoints.Length - 1) {
                        fromWaypointIndex = 0;
                        System.Array.Reverse(globalWaypoints);
                    }
                }
                nextMoveTime = Time.timeSinceLevelLoad + waitTime;
                
                if (useWithAnimator) {
                    moving = false; // angekommen
                    anim.SetBool(animatorStartParameter, false);
                    anim.SetBool(animatorStopParameter, true);
                }                
            }
            
            transform.Translate(pixelPerfectMoveAmount - transform.position);
        }
	}
    

	void OnDrawGizmos() {
		if (localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i =0; i < localWaypoints.Length; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i].position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
	
}
