using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableKeys: AbstractCollectable {

    public enum KEY_TYPE { SQUARE, CIRCLE, TRIANGLE };
    public KEY_TYPE keyType;

    public override void CollectItem() {
        PlayerStatistics.GetInstance().ModifyKeys(keyType, true);
    }
}
