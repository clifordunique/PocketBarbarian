using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePoints: AbstractCollectable {
    
    public IntValue points;

    public override void CollectItem() {
        Inventory.GetInstance().AddPoints(points.value);
    }
}