using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviour : MonoBehaviour {

    public EnemyAction[] actionQueue;

    private int currentQueuePosition = 0;

    public EnemyAction GetCurrentAction() {
        if (currentQueuePosition >= 0 && currentQueuePosition < actionQueue.Length) {
            return actionQueue[currentQueuePosition];
        }
        return null;
    }

    public EnemyAction GetNextAction() {
        
        currentQueuePosition++;
        if (currentQueuePosition >= actionQueue.Length) {
            currentQueuePosition = 0;
        }
        Debug.Log(">>>>>>>>>>> GET NEXT ACTION" + GetCurrentAction().actionEvent);
        return GetCurrentAction();
    }
}
