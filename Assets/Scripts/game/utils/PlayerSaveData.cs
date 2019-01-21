using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int lives;
    public int health;
    public int points;
    public float currentStamina;
    public bool hasSquareKey;
    public bool hasCircleKey;
    public bool hasTriangleKey;
    public string selectedPotionUuid;
    public string selectedAmmoUuid;

    public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();

}
