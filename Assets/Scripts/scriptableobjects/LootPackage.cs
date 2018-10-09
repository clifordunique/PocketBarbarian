using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootPackage : ScriptableObject {
    
    public GameObject[] prefabLootList;
    public int amountMin;
    public bool random = false;
    [ConditionalHideAttribute("random", true)]
    public int amountMax;
}
