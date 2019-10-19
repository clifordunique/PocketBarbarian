using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner: MonoBehaviour, ITriggerReactor {
    public GameObject enemyPrefab;
    public GameObject spawnEffectPrefab;
    public int pixelOffsetX;
    public int pixelOffsetY;
    public float timeOffsetSpawnEffect;
    public float timer;
    public float randomOffsetTimer;
    public bool randomDirection;
    public int maxEnemies;
    public int maxEnemiesAtOnce;

    public bool reactAsTriggerReactor = false;
    public bool killOnDeactivate = true;

    private float timeLastSpawn = 0;
    private float nextTimer;
    private int numberSpawnedEnemies = 0;

    private bool spawnAllowed = true;

    private Vector3 positionSpawnEffect;

    // Start is called before the first frame update
    void Start() {
        if (reactAsTriggerReactor) {
            spawnAllowed = false;
        }
        updateNextTimer();

        float offsetX = Utils.PixelToWorldunits(pixelOffsetX);
        float offsetY = Utils.PixelToWorldunits(pixelOffsetY);
        positionSpawnEffect = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

    }

    private void updateNextTimer() {
        nextTimer = timer + (Random.Range(-randomOffsetTimer, randomOffsetTimer));
    }

    // Update is called once per frame
    void Update() {
        if (spawnAllowed) {
            if ((maxEnemies <= 0 || maxEnemies > numberSpawnedEnemies) &&
                (maxEnemiesAtOnce <= 0 || transform.childCount < maxEnemiesAtOnce)) {

                if (timeLastSpawn + nextTimer < Time.timeSinceLevelLoad) {
                    //spawn

                    GameObject effect = Instantiate(spawnEffectPrefab, positionSpawnEffect, transform.rotation);
                    effect.transform.parent = EffectCollection.GetInstance().transform;

                    StartCoroutine(SpawnEnemy(timeOffsetSpawnEffect));


                    updateNextTimer();
                    timeLastSpawn = Time.timeSinceLevelLoad;
                    numberSpawnedEnemies++;
                }
            }
        }
    }

    private IEnumerator SpawnEnemy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        GameObject go = Instantiate(enemyPrefab, transform.position, transform.rotation);
        go.transform.parent = transform;
        if (randomDirection) {
            if (Random.value > 0.5f) {
                go.transform.localScale = new Vector3(-1 * go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
            }
        }
    }

    public bool TriggerActivated() {
        timeLastSpawn = Time.timeSinceLevelLoad;
        spawnAllowed = true;
        return true;
    }

    public bool TriggerDeactivated() {
        spawnAllowed = false;
        if (killOnDeactivate) {
            HurtBox[] hurtBoxes = GetComponentsInChildren<HurtBox>();
            foreach(HurtBox hurtBox in hurtBoxes) {
                hurtBox.ReceiveHit(true, 100, HitBox.DAMAGE_TYPE.DEFAULT, transform.position);
            }
        }
        return true;
    }
}
