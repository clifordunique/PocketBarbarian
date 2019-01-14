using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class ItemManager
{
    public void SaveShopItems() {
        ItemCollection si = new ItemCollection();
        Item item = new Item();
        item.itemType = Item.ITEM_TYPES.SWORD;
        item.name = "bla";
        item.uuid = "sss";
        item.shop = Item.SHOP_TYPES.BLACKSMITH;
        item.ammount = 1;
        Item.Effect effect = new Item.Effect();
        effect.effectType = Item.EFFECT_TYPES.DAMAGE;
        effect.value = 1F;
        item.effectList.Add(effect);
        
        si.items.Add(item);
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        string path = Application.dataPath + "/AllItems.xml";
        Debug.Log("path:" + path);
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, si);
        stream.Close();
    }

    public void LoadShopItems() {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        string path = Application.dataPath + "/AllItems.xml";
        var stream = new FileStream(path, FileMode.Open);
        ItemCollection container = serializer.Deserialize(stream) as ItemCollection;
        Debug.Log("ItemType:" + container.items[0].itemType);
        stream.Close();
    }
}
