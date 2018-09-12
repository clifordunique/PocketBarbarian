using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AiReactionAction{

    public enum REACTION_ACTION { MOVE, MOVE_SHOOT, SHOOT, MOVE_ACTION}
    public REACTION_ACTION action;

    public bool hitTargetIsVector = false;

    public float distanceSecondAction = 1F;

}
