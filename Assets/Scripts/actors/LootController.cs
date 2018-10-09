using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {
    [Header("Loot Death")]
    public LootPackage lootPackageDeath;

    [Header("Loot Hit")]
    public bool lootOnHit = false;
    [ConditionalHideAttribute("lootOnHit", true)]
    public LootPackage lootPackageHit;



    public void SpawnLootHit() {
        if (lootOnHit) {
            Spawn(lootPackageHit);
        }
    }

    public void SpawnLootDeath() {
        Spawn(lootPackageDeath);
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
