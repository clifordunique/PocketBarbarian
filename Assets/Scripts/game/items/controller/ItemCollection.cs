using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ItemCollection")]
public class ItemCollection
{
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<Item> items = new List<Item>();

    public List<Item> SearchItems(Item.SHOP_TYPES shopType) {
        List<Item> result = items.FindAll(x => x.shop.Equals(shopType));
        return result;
    }
}
