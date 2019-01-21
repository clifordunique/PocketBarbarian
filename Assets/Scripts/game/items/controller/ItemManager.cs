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


    // Refresh PlayerStatistics with Item Inventory
    public void RefreshPlayerStatisticsWithInventory(PlayerStatistics playerSt, Dictionary<string, Item> inventory) {

        // First read all items and add all effects
        foreach (KeyValuePair<string, Item> kvp in inventory) {
            UpdateEffectToPlayerStatistics(playerSt, kvp.Value);
        }
        

        // after that, validate max values and cut exceeding levels
        if (playerSt.maxHealth > playerSt.MAXMAXHEALTH) {
            playerSt.maxHealth = playerSt.MAXMAXHEALTH;
        }

        if (playerSt.health > playerSt.maxHealth) {
            playerSt.health = playerSt.maxHealth;
        }

        HandlePotions(playerSt, inventory);
        HandleAmmo(playerSt, inventory);
    }

    private void UpdateEffectToPlayerStatistics(PlayerStatistics playerSt, Item item) {
        List<Item.Effect> effectList = item.effectList;
        foreach (Item.Effect effect in effectList) {
            if (effect.effectType == Item.EFFECT_TYPES.MAX_HEALTH) {
                playerSt.maxHealth = playerSt.baseMaxHealth + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.MAX_POTIONS) {
                playerSt.maxPotions = playerSt.baseMaxPotions + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.MAX_AMMO) {
                playerSt.maxAmmo = playerSt.baseMaxAmmo + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.STAMINA) {
                playerSt.staminaActionCost = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.WALL_JUMP) {
                playerSt.wallJumpAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DOUBLE_JUMP) {
                playerSt.doubleJumpAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DASH) {
                playerSt.dashAllowed = true;
                playerSt.dashDuration = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.STOMP) {
                playerSt.stompAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.COMBO) {
                playerSt.comboAllowed = true;
                playerSt.comboLevel = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DAMAGE && item.itemType == Item.ITEM_TYPES.SWORD) {
                playerSt.hasWeapon = true;
                playerSt.damage = Mathf.RoundToInt(effect.value);
            }
        }        
    }

    public void HandlePotions(PlayerStatistics playerSt, Dictionary<string, Item> inventory) {

        List<Item> itemList = new List<Item>();
        foreach (KeyValuePair<string, Item> kvp in inventory) {
            itemList.Add(kvp.Value);
        }

        // check max ammounts
        List<Item> potions = Item.FindForItemType(itemList, Item.ITEM_TYPES.POTION);
        if (potions != null && potions.Count > 0) {
            // check max ammounts
            foreach (Item potion in potions) {
                if (potion.amount > playerSt.maxPotions) {
                    potion.amount = playerSt.maxPotions;
                }
            }

            Item selectedPotion = null;
            if (playerSt.selectedPotionUuid != null && playerSt.selectedPotionUuid != "") {
                selectedPotion = Item.FindForUuid(potions, playerSt.selectedPotionUuid);
            }
            if (selectedPotion == null) {
                // suche nach naechster Potion
                Item.Sort(potions);
                selectedPotion = potions[0];
            }
            if (selectedPotion != null) {
                playerSt.selectedPotionUuid = selectedPotion.uuid;
                playerSt.potions = selectedPotion.amount;
            }
        }
    }

    public void HandleAmmo(PlayerStatistics playerSt, Dictionary<string, Item> inventory) {

        List<Item> itemList = new List<Item>();
        foreach (KeyValuePair<string, Item> kvp in inventory) {
            itemList.Add(kvp.Value);
        }

        // check max ammounts
        List<Item> ammos = Item.FindForItemType(itemList, Item.ITEM_TYPES.AMMO);
        if (ammos != null && ammos.Count > 0) {
            // check max ammounts
            foreach (Item ammo in ammos) {
                if (ammo.amount > playerSt.maxAmmo) {
                    ammo.amount = playerSt.maxAmmo;
                }
            }

            Item selectedAmmo = null;
            if (playerSt.selectedAmmoUuid != null && playerSt.selectedAmmoUuid != "") {
                selectedAmmo = Item.FindForUuid(ammos, playerSt.selectedAmmoUuid);
            }
            if (selectedAmmo == null) {
                // suche nach naechster Potion
                Item.Sort(ammos);
                selectedAmmo = ammos[0];
            }
            if (selectedAmmo != null) {
                playerSt.selectedAmmoUuid = selectedAmmo.uuid;
                playerSt.ammo = selectedAmmo.amount;
            }
        }
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
        XmlSerializer serializer = new XmlSerializer(typeof(ItemCollection));
        string path = Application.dataPath + "/AllItems.xml";
        var stream = new FileStream(path, FileMode.Open);
        container = serializer.Deserialize(stream) as ItemCollection;
        Debug.Log("ItemType:" + container.items[0].itemType);
        stream.Close();
    }
}
