using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePotions : AbstractCollectable {

    public IntValue potions;

    public override void CollectItem() {
        if (potions != null) {
            PlayerStatistics.GetInstance().ModifyPotions(potions.value);
        }
    }
}
