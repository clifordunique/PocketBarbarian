using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChild: MonoBehaviour  {


    public void SpawnLoot(LootPackage lootPackage) {
        Spawn(lootPackage);
    }


    private void Spawn(LootPackage lootPackage) {
        int amount = lootPackage.amountMin;
        if (lootPackage.random) {
            amount = Random.Range(lootPackage.amountMin, lootPackage.amountMax);
        }
        StartCoroutine(SpawnTimed(amount, lootPackage));
    }

    IEnumerator SpawnTimed(int amount, LootPackage lootPackage) {
        Debug.Log("Amount Spawn:" + amount);
        for (int i = 0; i < amount; i++) {
            Debug.Log("Spawn " + i);
            int pos = Random.Range(0, lootPackage.prefabLootList.Length);
            InstantiateEffect(lootPackage.prefabLootList[pos]);
            Debug.Log("Instantiate complete ");
            yield return new WaitForSeconds(0.01F);
            Debug.Log("Waited for seconds " + i);
        }
        Destroy(gameObject);
    }


    private void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = transform.position;
    }


}
