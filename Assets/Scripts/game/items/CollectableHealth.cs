using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHealth : AbstractCollectable {

    public IntValue health;

    public override void CollectItem() {
        if (health != null) {
            PlayerStatistics.GetInstance().ModifyHealth(health.value);
        }
    }
}
