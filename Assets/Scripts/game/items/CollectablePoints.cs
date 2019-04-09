using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePoints: AbstractCollectable {
    
    public IntValue points;

    public override void CollectItem() {
        PlayerStatistics.GetInstance().ModifyPoints(points.value);

        if (collectableNumberEffect != null) {
            /*GameObject effect = InstantiateEffect(collectableNumberEffect);
            ShowSimpleText simpelText = effect.GetComponentInChildren<ShowSimpleText>();
            if (simpelText != null) {
                simpelText.text = "+" + points.value;
                Debug.Log("SIMPLETEXT FOUND!");
            } */
        }
    }
}