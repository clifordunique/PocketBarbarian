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
            Spawn(lootPackageMin);
        }
    }

    public void SpawnChildrenOnDeath() {
        Spawn(lootPackageMax);
    }


    private void Spawn(LootPackage lootPackage) {
        int amount = lootPackage.amountMin;
        if (lootPackage.random) {
            amount = Random.Range(lootPackage.amountMin, lootPackage.amountMax);
        }
        StartCoroutine(SpawnTimed(amount, lootPackage));
//        for (int i = 0; i < amount; i++) {
//            int pos = Random.Range(0, lootPackage.prefabLootList.Length);
//            InstantiateEffect(lootPackage.prefabLootList[pos]);
//        }
    }

    IEnumerator SpawnTimed(int amount, LootPackage lootPackage) {
        for (int i = 0; i < amount; i++) {
            int pos = Random.Range(0, lootPackage.prefabLootList.Length);
            InstantiateEffect(lootPackage.prefabLootList[pos]);
            yield return new WaitForSeconds(0.01F);
        }
        
    }


    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }


}
