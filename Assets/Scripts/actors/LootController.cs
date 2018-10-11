using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {
    [Header("Loot Max")]
    public LootPackage lootPackageMax;

    [Header("Loot Min")]
    public bool lootMin = false;
    [ConditionalHideAttribute("lootMin", true)]
    public LootPackage lootPackageMin;



    public void SpawnLootMin() {
        if (lootMin) {
            Spawn(lootPackageMin);
        }
    }

    public void SpawnLootMax() {
        Spawn(lootPackageMax);
    }

    private void Spawn(LootPackage lootPackage) {
        int amount = lootPackage.amountMin;
        if (lootPackage.random) {
            amount = Random.Range(lootPackage.amountMin, lootPackage.amountMax);
        }

        for (int i = 0; i < amount; i++) {
            int pos = Random.Range(0, lootPackage.prefabLootList.Length);
            InstantiateEffect(lootPackage.prefabLootList[pos]);
        }
    }


    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }
}
