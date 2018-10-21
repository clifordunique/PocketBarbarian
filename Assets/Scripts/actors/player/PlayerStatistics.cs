using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent: UnityEvent<int> { }

[System.Serializable]
public class FloatEvent: UnityEvent<float> { }

public class PlayerStatistics : MonoBehaviour {

    public int points;
    public int ammo;
    public int potions;
    public float stamina; // stamina in percent 0-1F
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

    private bool coroutineStamina = false;

    private static PlayerStatistics _instance;

    // Use this for initialization
    void Start () {
        _instance = this;        
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

}
