using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSlimeBig : EnemyController, ITriggerReactor
{
    public bool waitForTrigger = false;
    private AiBehaviour backupAiBehaviour;

    public override void Start() {
        if (waitForTrigger) {
            aiBehaviour = GetComponent<AiBehaviour>();
            backupAiBehaviour = aiBehaviour;
            aiBehaviour.enabled = false;
            aiBehaviour = null;
        }
        base.Start();
    }

    public override void ShootProjectile(Vector3 target, bool targetIsVector) {
        if (Time.timeSinceLevelLoad - lastShot > interval) {
            Vector3 spawnPosition1 = spawnPosition.position;
            float x = transform.position.x - spawnPosition.position.x;
            Vector3 spawnPosition2 = new Vector3(transform.position.x + x, spawnPosition.position.y, spawnPosition.position.z);
            

            GameObject projectileGo1 = Instantiate(projectile, spawnPosition1, transform.rotation, EffectCollection.GetInstance().transform);
            projectileGo1.GetComponent<Projectile>().InitProjectile(new Vector3(transform.localScale.x * target.x, target.y, target.z), targetIsVector, false);

            GameObject projectileGo2 = Instantiate(projectile, spawnPosition2, transform.rotation, EffectCollection.GetInstance().transform);
            projectileGo2.GetComponent<Projectile>().InitProjectile(new Vector3(transform.localScale.x * -1 * target.x, target.y, target.z), targetIsVector, false);
            lastShot = Time.timeSinceLevelLoad;
        }
    }

    public bool TriggerActivated() {
        if (waitForTrigger) {
            backupAiBehaviour.enabled = true;
            aiBehaviour = backupAiBehaviour;
            aiBehaviour.defaultAction = defaultAction;
            currentAction = aiBehaviour.GetCurrentAction();
        }
        return true;
    }

    public bool TriggerDeactivated() {
        return true;
    }
}
