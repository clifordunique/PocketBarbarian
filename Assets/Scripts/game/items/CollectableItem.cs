using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : AbstractCollectable {

    public string itemUuid;
    public int amount;

    public override void CollectItem() {
        Item originalItem = ItemManager.GetInstance().FindItem(itemUuid);
        Item collectedItem = new Item(originalItem);
        collectedItem.amount = amount;
        PlayerStatistics.GetInstance().ModifyItem(collectedItem);
    }
}
