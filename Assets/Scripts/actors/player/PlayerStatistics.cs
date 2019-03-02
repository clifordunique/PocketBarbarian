using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent: UnityEvent<int> { }

[System.Serializable]
public class IntIntEvent: UnityEvent<int, int> { }

[System.Serializable]
public class FloatEvent: UnityEvent<float> { }

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
    public int lives;
    public int health;
    public int points;
    public string weaponUuid;
    public int damage;
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


    [SerializeField]
    private IntIntEvent eventHealth;
    [SerializeField]
    private IntEvent eventPoints;
    [SerializeField]
    private FloatEvent eventStamina;
    [SerializeField]
    private KeysEvent eventKey;

    private bool coroutineStamina = false;

    private PlayerInventory inventoryManager;

    private static PlayerStatistics _instance;

    public static PlayerStatistics GetInstance() {
        return _instance;
    }

    // Use this for initialization
    void Start () {
        // init Health data ( later maybe overwritten with save data)
        maxHealth = baseMaxHealth;
        health = maxHealth;
        PublishHealth();

        inventoryManager = GetComponent<PlayerInventory>();
        if (!inventoryManager) {
            Debug.Log("Inventory Manager not found!");
        }
        InitBaseData();

        _instance = this;
        
        eventPoints.Invoke(points);        
    }

    void Update() {
        
        if (currentStamina < 1 && !coroutineStamina) {
            StartCoroutine(RegenerateStamina());
        }

        if (InputController.GetInstance().IsSwitchPotionsKeyDown()) {
            inventoryManager.SwitchPotions();
        }

        if (InputController.GetInstance().IsUsePotionsKeyDown()) {
            inventoryManager.UsePotion();
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

    public void PublishHealth() {
        // publish new health to hurtbox
        PlayerHurtBox.GetInstance().currentHealth = health;
        PlayerHurtBox.GetInstance().maxHealth = maxHealth;

        // publish health to gui
        eventHealth.Invoke(maxHealth, health);
    }
    
    public void ModifyHealth(int healthMod) {
        if (health + healthMod > maxHealth) {
            health = maxHealth;
        } else {
            if (health + healthMod < 0) {
                health = 0;
            } else {
                health += healthMod;
            }
        }        
        eventHealth.Invoke(maxHealth, health);
        if (healthMod > 0) {
            PlayerController.GetInstance().FlashOutline();
        }
    }

    public void ModifyPoints(int additionalPoints) {
        points += additionalPoints;
        eventPoints.Invoke(points);
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
        inventoryManager.AddItem(item);
        if (item.itemType == Item.ITEM_TYPES.SWORD) {
            // react to sword collect
            PlayerController.GetInstance().InterruptState(AbstractState.ACTION.SWORD_UP);
        }
    }    


    public PlayerSaveData CreateSaveData() {
        PlayerSaveData result = new PlayerSaveData();
        result.lives = lives;
        result.health = health;
        result.points = points;
        result.currentStamina = currentStamina;
        result.selectedPotionUuid = inventoryManager.selectedPotionUuid;
        result.selectedAmmoUuid = inventoryManager.selectedAmmoUuid;
        result.hasSquareKey = hasSquareKey;
        result.hasCircleKey = hasCircleKey;
        result.hasTriangleKey = hasTriangleKey;
        foreach(KeyValuePair<string, Item> kvp in inventoryManager.inventory) {
            // speichere Item UUID und Anzahl
            result.inventoryItems.Add(kvp.Key, kvp.Value.amount);
        }        
        return result;
    }

    public void RefreshSaveData(PlayerSaveData saveData) {
        lives = saveData.lives;
        health = saveData.health;
        inventoryManager.selectedPotionUuid = saveData.selectedPotionUuid;
        inventoryManager.selectedAmmoUuid = saveData.selectedAmmoUuid;

        ModifyPoints(saveData.points - points);         
        currentStamina = saveData.currentStamina;
        if (saveData.hasSquareKey) { ModifyKeys(CollectableKeys.KEY_TYPE.SQUARE, true); }
        if (saveData.hasCircleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.CIRCLE, true); }
        if (saveData.hasTriangleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.TRIANGLE, true); }

        DictionaryOfUuidAndItem inventoryItems = new DictionaryOfUuidAndItem();
        foreach (KeyValuePair<string, int> kvp in saveData.inventoryItems) {
            Item originalItem = ItemManager.GetInstance().FindItem(kvp.Key);
            Item myItem = new Item(originalItem);
            myItem.amount = kvp.Value;
            inventoryItems.Add(myItem.uuid, myItem);
        }
        
        inventoryManager.InitInventory(inventoryItems);
    }


    public void InitBaseData() {

        maxHealth = baseMaxHealth;
        maxPotions = baseMaxPotions;
        maxAmmo = baseMaxAmmo;
        staminaActionCost = baseStaminaActionCost;
        damage = 0;
        weaponUuid = "-1";
        wallJumpAllowed = false;
        doubleJumpAllowed = false;
        dashAllowed = false;
        dashDuration = 0;
        stompAllowed = false;
        comboAllowed = false;
        comboLevel = 0;

        PublishBaseData();
    }

    public void PublishBaseData() {
        PublishHealth();
        // publish weapon
        PlayerController.GetInstance().SetWeapon(weaponUuid, damage);

        // publish moveData
        PlayerMoveController2D moveController = PlayerController.GetInstance().moveController;
        moveController.wallJumpingAllowed = wallJumpAllowed;
        moveController.doubleJumpAllowed = doubleJumpAllowed;
        moveController.dashAllowed = dashAllowed;
        moveController.dashDuration = dashDuration;
        moveController.stampingAllowed = stompAllowed;

        PlayerController.GetInstance().comboAllowed = comboAllowed;
    }
}
