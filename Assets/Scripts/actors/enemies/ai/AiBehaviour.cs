using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviour : MonoBehaviour {

    
    public bool circular = true;

    public EnemyAction defaultAction;
    private EnemyAction[] actionQueue = null;
    private int currentQueuePosition = 0;

    public virtual void Awake() {
        initActionQueue();
    }

    private void initActionQueue() {
        if (actionQueue == null) {
            // get Waypoint children
            AiWaypoint[] waypoints = transform.GetComponentsInChildren<AiWaypoint>();
            // extract all Actions
            actionQueue = new EnemyAction[waypoints.Length];
            int i = 0;
            foreach (AiWaypoint waypoint in waypoints) {
                actionQueue[i++] = waypoint.action;
            }
        }
    }

    public virtual EnemyAction GetCurrentAction() {
        initActionQueue();
        if (currentQueuePosition >= 0 && currentQueuePosition < actionQueue.Length) {
            return actionQueue[currentQueuePosition];
        }
        return defaultAction;
    }

    public virtual EnemyAction GetNextAction() {
        
        currentQueuePosition++;
        if (currentQueuePosition >= actionQueue.Length) {
            if (!circular) {
                System.Array.Reverse(actionQueue);                
            }
            currentQueuePosition = 0;
        }
        return GetCurrentAction();
    }

    void OnDrawGizmos() {
        AiWaypoint[] waypointsGz = transform.GetComponentsInChildren<AiWaypoint>();
        if (waypointsGz != null) {
            Gizmos.color = Color.blue;
            float size = .3f;

            for (int i = 0; i < waypointsGz.Length; i++) {
                Vector3 globalWaypointPos = (Application.isPlaying) ? actionQueue[i].moveTarget : waypointsGz[i].transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
