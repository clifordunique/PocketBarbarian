using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePotions : AbstractCollectable {

    public enum POTION_TYPE { SMALL_POTION, MEDIUM_POTION, BIG_POTION };
    public POTION_TYPE potionType;
    public IntValue potions;

    public override void CollectItem() {
        if (potions != null) {
            PlayerStatistics.GetInstance().ModifyPotions(potionType, potions.value);
        }
    }
}
