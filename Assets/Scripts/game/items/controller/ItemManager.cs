using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class ItemManager
{
    public ItemCollection container;

    private static ItemManager _instance;
    

    public static ItemManager GetInstance() {
        if (_instance != null) {
            return _instance;
        } else {
            Debug.LogError("Item Manager not yet instantiated!");
        }
        return null;
    }

    public ItemManager() {
        _instance = this;
    }

    public Item FindItem(string itemUuid) {
        return Item.FindForUuid(container.items, itemUuid);
    }


  

    public void SaveShopItems() {
        /*
        ItemCollection si = new ItemCollection();
        Item item = new Item();
        item.itemType = Item.ITEM_TYPES.POTION;
        
        item.name = "Health Potion Medium";
        item.uuid = "sss";
        item.shop = Item.SHOP_TYPES.TRADER;
        item.amount = 10;
        Item.Effect effect = new Item.Effect();
        effect.effectType = Item.EFFECT_TYPES.HEALTH;
        effect.value = 2F;
        item.effectList.Add(effect);
        
        si.items.Add(item);
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        string path = Application.dataPath + "/AllItems.xml";
        Debug.Log("path:" + path);
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, si);
        stream.Close();
        */
    }

    public void LoadShopItems() {
        Debug.Log("LOAD SHOP ITEMS 0");
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        TextAsset textAsset = Resources.Load("Items/AllItems") as TextAsset;
        Debug.Log("LOAD SHOP ITEMS 1");
        Stream stream = new MemoryStream(textAsset.bytes);
        Debug.Log("LOAD SHOP ITEMS 2");
        container = serializer.Deserialize(stream) as ItemCollection;
        Debug.Log("LOAD SHOP ITEMS 3");
        Debug.Log("ItemType:" + container.items[0].itemType);
        stream.Close();
    }
}
