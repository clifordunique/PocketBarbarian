using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


public class Item
{
    public enum ITEM_TYPES {SWORD, POTION, AMMO, BOOTS, AMULETTE, BAG, PERMANENT_EFFECT };
    public enum SHOP_TYPES { BLACKSMITH, MAGE, TRADER, CHURCH };
    public enum EFFECT_TYPES { LIVES, HEALTH, STAMINA, MAX_POTIONS, MAX_AMMO, DAMAGE, WALL_JUMP, DOUBLE_JUMP, DASH, STOMP, COMBO}

    [XmlElement("UUID")]
    public string uuid;

    [XmlElement("ItemType")]
    public ITEM_TYPES itemType;

    [XmlElement("Shop")]
    public SHOP_TYPES shop;

    [XmlElement("Name")]
    public string name;

    [XmlElement("Ammount")]
    public int ammount;


    [XmlArray("Effects")]
    [XmlArrayItem("Effect")]
    public List<Effect> effectList = new List<Effect>();


    public class Effect {
        public EFFECT_TYPES effectType;
        public float value;
    }
}
