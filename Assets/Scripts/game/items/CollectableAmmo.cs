using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAmmo : AbstractCollectable {

    public IntValue ammo;

    public override void CollectItem() {
        if (ammo != null) {
            PlayerStatistics.GetInstance().ModifyAmmo(ammo.value);
        }
    }
}
