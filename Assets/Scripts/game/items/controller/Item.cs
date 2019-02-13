using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Item
{
    public enum ITEM_TYPES {SWORD, POTION, AMMO, BOOTS, AMULETTE, BAG, PERMANENT_EFFECT };
    public enum SHOP_TYPES { BLACKSMITH, MAGE, TRADER, CHURCH };
    public enum EFFECT_TYPES { LIVES, MAX_HEALTH, MAX_POTIONS, MAX_AMMO, HEALTH, STAMINA, DAMAGE, WALL_JUMP, DOUBLE_JUMP, DASH, STOMP, COMBO}

    [XmlElement("UUID")]
    public string uuid;

    [XmlElement("ItemType")]
    public ITEM_TYPES itemType;

    [XmlElement("Priority")]
    public int priority;

    [XmlElement("Shop")]
    public SHOP_TYPES shop;

    [XmlElement("Name")]
    public string name;

    [XmlElement("Stackable")]
    public bool stackable;

    [XmlElement("Amount")]
    public int amount;


    [XmlArray("Effects")]
    [XmlArrayItem("Effect")]
    public List<Effect> effectList = new List<Effect>();


    public Item() {
    }

    public Item(Item original) {
        this.uuid = original.uuid;
        this.itemType = original.itemType;
        this.priority = original.priority;
        this.shop = original.shop;
        this.name = original.name;
        this.stackable = original.stackable;
        this.amount = original.amount;
        this.effectList = original.effectList;
    }

    [System.Serializable]
    public class Effect {
        public EFFECT_TYPES effectType;
        public float value;
    }

    public static Item FindForUuid(List<Item> itemList, string uuid) {
        Item result = itemList.Find(x => x.uuid.Equals(uuid));
        return result;
    }

    public static List<Item> FindForItemType(List<Item> itemList, ITEM_TYPES itemType) {
        List<Item> result = itemList.FindAll(x => x.itemType.Equals(itemType));
        return result;
    }
    
    public static List<Item> SearchItemsWithUuids(List<Item> items, List<string> uuids) {

        List<Item> itemsResult = new List<Item>();
        foreach (string uuid in uuids) {
            Item result = items.Find(x => x.uuid.Equals(uuid));
            if (result != null) {
                itemsResult.Add(result);
            }
        }

        return itemsResult;
    }

    public static List<Item> SearchItemsForShop(List<Item> items, Item.SHOP_TYPES shopType) {
        List<Item> result = items.FindAll(x => x.shop.Equals(shopType));
        return result;
    }

    public static void Sort(List<Item> items) {
        items.Sort((p1, p2) => p1.priority.CompareTo(p2.priority));
    }
}
