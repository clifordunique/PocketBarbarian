using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent: UnityEvent<int> { }

[System.Serializable]
public class FloatEvent: UnityEvent<float> { }

[System.Serializable]
public class KeysEvent: UnityEvent<CollectableKeys.KEY_TYPE, bool> { }

public class PlayerStatistics : MonoBehaviour {
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
    public int potions;
    public int ammo;
    public int points;
    public float currentStamina; // stamina in percent 0-1F

    public bool hasSquareKey;
    public bool hasCircleKey;
    public bool hasTriangleKey;

    [ReadOnly]
    public List<string> inventoryItemUuids;
    public List<Item> inventory;    


    [SerializeField]
    private IntEvent eventPoints;
    [SerializeField]
    private IntEvent eventAmmo;
    [SerializeField]
    private IntEvent eventPotions;
    [SerializeField]
    private FloatEvent eventStamina;
    [SerializeField]
    private KeysEvent eventKey;

    private bool coroutineStamina = false;

    private static PlayerStatistics _instance;

    // Use this for initialization
    void Start () {
        _instance = this;
        eventAmmo.Invoke(ammo);
        eventPoints.Invoke(points);
        eventPotions.Invoke(potions);
    }

    void Update() {
        
        if (currentStamina < 1 && !coroutineStamina) {
            StartCoroutine(RegenerateStamina());
        }
    }

    IEnumerator RegenerateStamina() {
        coroutineStamina = true;
        while (currentStamina < 1) {
            currentStamina += stepsStaminaRegeneration;
            eventStamina.Invoke(currentStamina);
            yield return new WaitForSeconds(timeStaminaRegeneration);
        }
        coroutineStamina = false;
    }

    public static PlayerStatistics GetInstance() {
        return _instance;
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

    public void ModifyAmmo(int modifyAmmo) {
        ammo += modifyAmmo;
        eventAmmo.Invoke(ammo);
    }

    public void ModifyPoints(int additionalPoints) {
        points += additionalPoints;
        eventPoints.Invoke(points);
    }

    public void ModifyPotions(int modifyPotions) {
        potions += modifyPotions;
        eventPotions.Invoke(potions);
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




    public PlayerSaveData CreateSaveData() {
        PlayerSaveData result = new PlayerSaveData();
        result.lives = lives;
        result.health = health;
        result.potions = potions;
        result.ammo = ammo;
        result.points = points;
        result.currentStamina = currentStamina;
        result.hasSquareKey = hasSquareKey;
        result.hasCircleKey = hasCircleKey;
        result.hasTriangleKey = hasTriangleKey;
        result.itemUuids = inventoryItemUuids;
        return result;
    }

    public void RefreshSaveData(PlayerSaveData saveData) {

        lives = saveData.lives;
        health = saveData.health;
        ModifyPotions(saveData.potions - potions);
        ModifyAmmo(saveData.ammo - ammo);
        ModifyPoints(saveData.points - points);         
        currentStamina = saveData.currentStamina;
        if (saveData.hasSquareKey) { ModifyKeys(CollectableKeys.KEY_TYPE.SQUARE, true); }
        if (saveData.hasCircleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.CIRCLE, true); }
        if (saveData.hasTriangleKey) { ModifyKeys(CollectableKeys.KEY_TYPE.TRIANGLE, true); }

        inventoryItemUuids = saveData.itemUuids;
    }
}
