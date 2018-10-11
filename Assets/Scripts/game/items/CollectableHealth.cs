using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHealth : AbstractCollectable {

    public IntValue health;

    public override void CollectItem() {
        if (health != null) {
            PlayerHurtBox.GetInstance().ModifyHealth(health.value);
        }
    }
}
