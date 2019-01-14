using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int lives;
    public int health;
    public int potions;
    public int ammo;
    public int points;
    public float currentStamina;
    public bool hasSquareKey;
    public bool hasCircleKey;
    public bool hasTriangleKey;

    public List<string> itemUuids;
    
}
