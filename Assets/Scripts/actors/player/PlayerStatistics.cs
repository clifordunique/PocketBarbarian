using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent: UnityEvent<int> { }

public class PlayerStatistics : MonoBehaviour {

    public int points;
    public int ammo;
    public int potions;
    public int stamina;
    public float timeStaminaRegeneration;

    [SerializeField]
    private IntEvent eventPoints;
    [SerializeField]
    private IntEvent eventAmmo;
    [SerializeField]
    private IntEvent eventPotions;
    [SerializeField]
    private IntEvent eventStamina;

    private bool coroutineStamina = false;

    private static PlayerStatistics _instance;

    // Use this for initialization
    void Start () {
        _instance = this;        
	}

    void Update() {
        
        if (stamina < 22 && !coroutineStamina) {
            StartCoroutine(RegenerateStamina());
        }
    }

    IEnumerator RegenerateStamina() {
        Debug.Log("Regenerate Stamina start");
        coroutineStamina = true;
        while (stamina < 22) {
            stamina++;
            Debug.Log("Regenerate Stamina Invoke Event!");
            eventStamina.Invoke(stamina);
            yield return new WaitForSeconds(timeStaminaRegeneration);
        }
        coroutineStamina = false;
    }

    public static PlayerStatistics GetInstance() {
        return _instance;
    }

    public void ModifyStamina(int modifyStamina) {
        stamina = (stamina + modifyStamina < 0 ? 0 : stamina + modifyStamina);
        
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
