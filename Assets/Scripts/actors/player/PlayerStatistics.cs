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

    public int points;
    public int ammo;
    public int potions;
    public float stamina; // stamina in percent 0-1F
    public bool hasSquareKey;
    public bool hasCircleKey;
    public bool hasTriangleKey;

    public float staminaCostForDash = 0.5F;
    public float staminaCostForStomp = 0.5F;

    public float timeStaminaRegeneration;
    public float stepsStaminaRegeneration;

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
        
        if (stamina < 1 && !coroutineStamina) {
            StartCoroutine(RegenerateStamina());
        }
    }

    IEnumerator RegenerateStamina() {
        coroutineStamina = true;
        while (stamina < 1) {
            stamina += stepsStaminaRegeneration;
            eventStamina.Invoke(stamina);
            yield return new WaitForSeconds(timeStaminaRegeneration);
        }
        coroutineStamina = false;
    }

    public static PlayerStatistics GetInstance() {
        return _instance;
    }

    public void ModifyStamina(float modifyStaminaPercent) {
        stamina = (stamina + modifyStaminaPercent < 0 ? 0 : stamina + modifyStaminaPercent);
        
        eventStamina.Invoke(stamina);
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


    public bool HasEnoughStaminaForDash() {
        if (stamina - staminaCostForDash >= 0) {
            return true;
        }
        return false;
    }

    public bool HasEnoughStaminaForStomp() {
        if (stamina - staminaCostForStomp >= 0) {
            return true;
        }
        return false;
    }


    public void ReduceStaminaForDash() {
        ModifyStamina(-staminaCostForDash);
    }

    public void ReduceStaminaForStomp() {
        ModifyStamina(-staminaCostForStomp);
    }
}
