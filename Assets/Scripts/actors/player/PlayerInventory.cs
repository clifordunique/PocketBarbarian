using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemEvent: UnityEvent<Item> { }


public class PlayerInventory : MonoBehaviour
{
    private const string EMPTY_POTION = "EMPTY_POTION";
    private const string EMPTY_AMMO = "EMPTY_AMMO";
    public string selectedPotionUuid;
    public string selectedAmmoUuid;

    private PlayerStatistics statistics;


    [SerializeField]
    public ItemEvent eventItem;


    [SerializeField]
    public DictionaryOfUuidAndItem inventory = new DictionaryOfUuidAndItem();

    // Start is called before the first frame update
    void Start()    {
        statistics = GetComponent<PlayerStatistics>();
        if (!statistics) {
            Debug.Log("PlayerStatistics not found!");
        }
    }

    public void InitInventory(DictionaryOfUuidAndItem loadInventory) {

        statistics.InitBaseData();

        inventory = loadInventory;

        // First read all items and add all effects
        foreach (KeyValuePair<string, Item> kvp in inventory) {
            UpdateEffectToPlayerStatistics(kvp.Value, true);
        }

        // second check max values
        foreach (KeyValuePair<string, Item> kvp in inventory) {
            CheckMaxValuesInInventory(kvp.Value);
        }

        // publish all statistic data to all depending components
        statistics.PublishBaseData();

        // publish items to display
        PublishSelectedItems();
    }

    public void UsePotion() {
        Item activePotion = inventory[selectedPotionUuid];
        if (activePotion != null && activePotion.amount > 0) {
            activePotion.amount--;
            UpdateEffectToPlayerStatistics(activePotion, false);
            PlayerController.GetInstance().FlashOutline();
        }
        if (activePotion.amount <= 0) {
            // remove potion
            inventory.Remove(activePotion.uuid);
            SwitchPotions();
        }
        statistics.PublishHealth();
    }

    public void SwitchPotions() {
        List<Item> potionList = InventoryToList(Item.ITEM_TYPES.POTION);
        if (potionList == null || potionList.Count <= 0) {
            selectedPotionUuid = "";
        } else {
            int pos = -1;
            if (inventory.ContainsKey(selectedPotionUuid)) {
                pos = potionList.IndexOf(inventory[selectedPotionUuid]);
            }
            pos++;
            if (potionList.Count <= pos) {
                pos = 0;
            }
            Item newCurrentPotion = potionList[pos];
            selectedPotionUuid = newCurrentPotion.uuid;
        }
        PublishSelectedItems();
    }

    public void AddItem(Item item) {
        // search for existing item
        Item myItem = null;
        if (item != null) {
            if (inventory.ContainsKey(item.uuid)) {
                // item gefunden
                myItem = inventory[item.uuid];

                if (myItem.stackable) {
                    // item darf gestackt werden
                    myItem.amount += item.amount;
                }
            } else {
                // item nicht gefunden, hinzufuegen
                myItem = new Item(item);
                inventory.Add(myItem.uuid, myItem);
            }

            UpdateEffectToPlayerStatistics(myItem, true);
            CheckMaxValuesInInventory(myItem);
        }
        // publish all statistic data to all depending components
        statistics.PublishBaseData();

        UpdateSelectedItem(myItem);
    }

    private void UpdateSelectedItem(Item item) {
        if (item.itemType == Item.ITEM_TYPES.POTION) {            
            if (inventory.ContainsKey(selectedPotionUuid)) {
                Item selectedItem = inventory[selectedPotionUuid];
                if (selectedItem.uuid == item.uuid || item.priority < selectedItem.priority ) {
                    selectedPotionUuid = item.uuid;
                }
            } else {
                selectedPotionUuid = item.uuid;
            }
        } else 
        if (item.itemType == Item.ITEM_TYPES.AMMO) {
            if (inventory.ContainsKey(selectedAmmoUuid)) {
                Item selectedItem = inventory[selectedAmmoUuid];
                if (selectedItem.uuid == item.uuid || item.priority >= selectedItem.priority) {
                    selectedAmmoUuid = item.uuid;
                }
            } else {
                selectedAmmoUuid = item.uuid;
            }
        }
        PublishSelectedItems();
    }

    /// <summary>
    ///  Publishes current potions and ammo to gui
    /// </summary>
    private void PublishSelectedItems() {
        Item potionToPublish = null;
        if (selectedPotionUuid.Length > 0 && inventory.ContainsKey(selectedPotionUuid)) {
            potionToPublish = inventory[selectedPotionUuid];
        } else {
            potionToPublish = new Item();
            potionToPublish.uuid = EMPTY_POTION;
        }
        eventItem.Invoke(potionToPublish);

        Item ammoToPublish = null;
        if (selectedAmmoUuid.Length > 0 && inventory.ContainsKey(selectedAmmoUuid)) {
            ammoToPublish = inventory[selectedPotionUuid];
        } else {
            ammoToPublish = new Item();
            ammoToPublish.uuid = EMPTY_AMMO;
        }
        
        eventItem.Invoke(ammoToPublish);
    }



    private void CheckMaxValuesInInventory(Item item) {
        if (statistics.maxHealth > statistics.MAXMAXHEALTH) {
            statistics.maxHealth = statistics.MAXMAXHEALTH;
        }

        if (item.itemType == Item.ITEM_TYPES.POTION) {
            if (item.amount > statistics.maxPotions) {
                item.amount = statistics.maxPotions;
            }
        }

        if (item.itemType == Item.ITEM_TYPES.AMMO) {
            if (item.amount > statistics.maxAmmo) {
                item.amount = statistics.maxAmmo;
            }
        }
    }

    private void UpdateEffectToPlayerStatistics(Item item, bool equipOnly) {
        List<Item.Effect> effectList = item.effectList;
        foreach (Item.Effect effect in effectList) {
            if (effect.effectType == Item.EFFECT_TYPES.MAX_HEALTH) {
                statistics.maxHealth = statistics.baseMaxHealth + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.MAX_POTIONS) {
                statistics.maxPotions = statistics.baseMaxPotions + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.MAX_AMMO) {
                statistics.maxAmmo = statistics.baseMaxAmmo + Mathf.RoundToInt(effect.value);
            }
            if (effect.effectType == Item.EFFECT_TYPES.STAMINA) {
                statistics.staminaActionCost = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.WALL_JUMP) {
                statistics.wallJumpAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DOUBLE_JUMP) {
                statistics.doubleJumpAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DASH) {
                statistics.dashAllowed = true;
                statistics.dashDuration = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.STOMP) {
                statistics.stompAllowed = true;
            }
            if (effect.effectType == Item.EFFECT_TYPES.COMBO) {
                statistics.comboAllowed = true;
                statistics.comboLevel = effect.value;
            }
            if (effect.effectType == Item.EFFECT_TYPES.DAMAGE && item.itemType == Item.ITEM_TYPES.SWORD) {                
                statistics.weaponUuid = item.uuid;
                statistics.damage = Mathf.RoundToInt(effect.value);
            }
            if (!equipOnly) {
                if (effect.effectType == Item.EFFECT_TYPES.HEALTH) {
                    if ((statistics.health + Mathf.RoundToInt(effect.value)) > statistics.maxHealth) {
                        statistics.health = statistics.maxHealth;
                    } else {
                        statistics.health += Mathf.RoundToInt(effect.value);
                    }
                }
            }
        }
    }

    private List<Item> InventoryToList(Item.ITEM_TYPES itemType) {
        List<Item> result = new List<Item>();
        foreach(KeyValuePair<string, Item> kvp in inventory) {
            if (kvp.Value.itemType == itemType) {
                result.Add(kvp.Value);
            }            
        }
        result.Sort((p1, p2) => -(p1.priority.CompareTo(p2.priority)));
        return result;
    }
}
