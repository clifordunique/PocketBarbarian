using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ShopItems")]
public class ShopItems{

    [XmlElement("ShopName")]
    public string shopName;

    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<Item> items = new List<Item>();

}
