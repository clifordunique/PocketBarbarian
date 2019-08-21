using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChildGenerator: MonoBehaviour, IChildGenerator  {
    [Header("Loot Max")]
    public LootPackage lootPackageMax;

    [Header("Loot Min")]
    public bool lootMin = false;
    [ConditionalHideAttribute("lootMin", true)]
    public LootPackage lootPackageMin;

    public void SpawnChildrenOnHit() {
        if (lootMin) {
            InstantiateLootChild(lootPackageMin);
        }
    }

    public void SpawnChildrenOnDeath() {
        InstantiateLootChild(lootPackageMax);
    }


    private void InstantiateLootChild(LootPackage lootPackage) {

        GameObject objToSpawn = new GameObject("LootChild_" + name);
        //Add Components
        objToSpawn.AddComponent<LootChild>();
        objToSpawn.transform.position = transform.position;
        objToSpawn.transform.parent = EffectCollection.GetInstance().transform;
        LootChild lc = objToSpawn.GetComponent<LootChild>();
        lc.SpawnLoot(lootPackage);
    }


}
