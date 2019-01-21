using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent: UnityEvent<int> { }

[System.Serializable]
public class FloatEvent: UnityEvent<float> { }

[System.Serializable]
public class PotionsEvent: UnityEvent<CollectablePotions.POTION_TYPE, int> { }

[System.Serializable]
public class KeysEvent: UnityEvent<CollectableKeys.KEY_TYPE, bool> { }

public class PlayerStatistics : MonoBehaviour {

    // absolute obergrenze fuer health
    [ReadOnly]
    public int MAXMAXHEALTH = 6;

    [Header("BaseSettings")]
    [ReadOnly]
    public int baseMaxHealth = 3;
    [ReadOnly]
    public int baseMaxPotions = 3;
    [ReadOnly]
    public int baseMaxAmmo = 5;
    [ReadOnly]
    public float baseStaminaActionCost = 1F;

    [Header("ModifiedBaseSettings")]
    public int maxHealth;
    public int maxPotions;
    public int maxAmmo;
    public float staminaActionCost;

    public float timeStaminaRegeneration;
    public float stepsStaminaRegeneration;

    [Header("Variable Properties")]
    public string selectedPotionUuid;
    public string selectedAmmoUuid;

    public int lives;
    public int health;
    public int potions;
    public int ammo;
    public int points;
    public int damage;
    public bool hasWeapon;
    public bool wallJumpAllowed;
    public bool doubleJumpAllowed;
    public bool dashAllowed;
    public float dashDuration;
    public bool stompAllowed;
    public bool comboAllowed;
    public float comboLevel;

    public float currentStamina; // stamina in percent 0-1F

    public bool hasSquareKey;
    public bool hasCircleKey;
    public bool hasTriangleKey;

    public Dictionary<string, Item> inventoryItems = new Dictionary<string, Item>();    


    [SerializeField]
    private IntEvent eventPoints;
    [SerializeField]
    private IntEvent eventAmmo;
    [SerializeField]
    private PotionsEvent eventPotions;
    [SerializeField]
    private FloatEvent eventStamina;
    [SerializeField]
    private KeysEvent eventKey;

    private bool coroutineStamina = false;

    private static PlayerStatistics _instance;

    public static PlayerStatistics GetInstance() {
        return _instance;
    }

    // Use this for initialization
    void Start () {
        InitBaseData();

        _instance = this;


        eventAmmo.Invoke(ammo);
        eventPoints.Invoke(points);
        eventPotions.Invoke(CollectablePotions.POTION_TYPE.SMALL_POTION, potions);
    }

    void Update() {
        
        if (currentStamina < 1 && !coroutineStamina) {
            StartCoroutine(RegenerateStamina());
        }
    }

    // **************************************************************
    // STAMINA - BEGIN
    // **************************************************************

    IEnumerator RegenerateStamina() {
        coroutineStamina = true;
        while (currentStamina < 1) {
            currentStamina += stepsStaminaRegeneration;
            eventStamina.Invoke(currentStamina);
            yield return new WaitForSeconds(timeStaminaRegeneration);
        }
        coroutineStamina = false;
    }

    public void ModifyStamina(float modifyStaminaPercent) {
        currentStamina = (currentStamina + modifyStaminaPercent < 0 ? 0 : currentStamina + modifyStaminaPercent);
        
        eventStamina.Invoke(currentStamina);
    }

    public bool HasEnoughStaminaForAction() {
        if (currentStamina - staminaActionCost >= 0) {
            return true;
        }
        return false;
    }

    public void ReduceStaminaForAction() {
        ModifyStamina(-staminaActionCost);
    }

    // **************************************************************
    // STAMINA - END
    // **************************************************************

    public void ModifyAmmo(int modifyAmmo) {
        ammo += modifyAmmo;
        eventAmmo.Invoke(ammo);
    }

    public void ModifyPoints(int additionalPoints) {
        points += additionalPoints;
        eventPoints.Invoke(points);
    }

    public void ModifyPotions(CollectablePotions.POTION_TYPE potionType, int modifyPotions) {
        potions += modifyPotions;
        eventPotions.Invoke(potionType, potions);
    }

    public void ModifyKeys(CollectableKeys.KEY_TYPE keytype, bool equip) {
        if (keytype == CollectableKeys.KEY_TYPE.SQUARE) {
            hasSquareKey = equip;
        }
        if (keytype == CollectableKeys.KEY_TYPE.CIRCLE) {
            hasCircleKey = equip;
        }
        if (keytype == CollectableKeys.KEY_TYPE.TRIANGLE) {
            hasTriangleKey = equip;
        }
        eventKey.Invoke(keytype, equip);
    }


    public void ModifyItem(Item item) {
        // search for existing item
        bool inventoryChanged = false;
        Item myItem = null;
        if (item != null) {
            if (inventoryItems.ContainsKey(item.uuid)) {
                myItem = inventoryItems[item.uuid];
                // item gefunden
                if (myItem.stackable) {
                    // item darf gestackt werden
                    inventoryChanged = AddAmount(myItem, item.amount);
                }
            } else {
                // item nicht gefunden, hinzufuegen
                myItem = new Item(item);
                inventoryItems.Add(myItem.uuid, myItem);
                inventoryChanged = true;
            }

            if (inventoryChanged) {
                Debug.Log("Refresh Player Inventory");
                InitBaseData();
                ItemManager.GetInstance().RefreshPlayerStatisticsWithInventory(this, inventoryItems);
            }
        }        
    }    

    private bool AddAmount(Item item, int newAmount) {
        if (item.itemType == Item.ITEM_TYPES.AMMO)
            return AddAmount(item, newAmount, maxAmmo);
        if (item.itemType == Item.ITEM_TYPES.POTION)
            return AddAmount(item, newAmount, maxPotions);
        if (item.itemType == Item.ITEM_TYPES.AMULETTE)
            return AddAmount(item, newAmount, MAXMAXHEALTH);
        return false;
    }

    private bool AddAmount(Item item, int newAmount, int max) {
        if (item.amount >= max || newAmount <= 0) {
            return false;
        }
        if (item.amount + newAmount > max) {
            item.amount = max;
            return true;
        } else {
            item.amount += newAmount;
            return true;
        }
    }

    public PlayerSaveData CreateSaveData() {
        PlayerSaveData result = new PlayerSaveData();
        result.lives = lives;
        result.health = health;
        result.points = points;
        result.currentStamina = currentStamina;
        result.selectedPotionUuid = selectedPotionUuid;
        result.selectedAmmoUuid = selectedAmmoUuid;
        result.hasSquareKey = hasSquareKey;
        result.hasCircleKey = hasCircleKey;
        result.hasTriangleKey = hasTriangleKey;
        foreach(KeyValuePair<string, Item> kvp in inventoryItems) {
            // speichere Item UUID und Anzahl
            result.inventoryItems.Add(kvp.Key, kvp.Value.amount);
        }        
        return result;
    }

    public void RefreshSaveData(PlayerSaveData saveData) {
        Debug.Log("Refresh SaveData");
        lives = saveData.lives;
        health = saveData.health;
        selectedPotionUuid = saveData.selectedPotionUuid;
        selectedAmmoUuid = saveData.selectedAmmoUuid;

        ModifyPoints(saveData.points - points);         
        currentStamina = saveData.currentStamina;
        if (saveData.hasSquareKey) { ModifyKeys(CollectableKeys.KEY_TYPE.SQUARE, true); }
        if (saveData.hasCircleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.CIRCLE, true); }
        if (saveData.hasTriangleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.TRIANGLE, true); }

        inventoryItems = new Dictionary<string, Item>();
        foreach (KeyValuePair<string, int> kvp in saveData.inventoryItems) {
            Item originalItem = ItemManager.GetInstance().FindItem(kvp.Key);
            Item myItem = new Item(originalItem);
            myItem.amount = kvp.Value;
            inventoryItems.Add(myItem.uuid, myItem);
        }

        InitBaseData();
        ItemManager.GetInstance().RefreshPlayerStatisticsWithInventory(this, inventoryItems);
    }


    private void InitBaseData() {

        maxHealth = baseMaxHealth;
        maxPotions = baseMaxPotions;
        maxAmmo = baseMaxAmmo;
        staminaActionCost = baseStaminaActionCost;

        potions = 0;
        ammo = 0;
        damage = 0;
        hasWeapon = false;
        wallJumpAllowed = false;
        doubleJumpAllowed = false;
        dashAllowed = false;
        dashDuration = 0;
        stompAllowed = false;
        comboAllowed = false;
        comboLevel = 0;
    }
}
