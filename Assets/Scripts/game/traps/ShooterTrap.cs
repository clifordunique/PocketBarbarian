using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrap : MonoBehaviour
{
    public GameObject spawnPositionProjectile;
    public GameObject prefabProjectile;

    public Sprite warmUpSprite;
    public Sprite shootSprite;

    public float warmUpTime = 0F;
    public float shootTime = 0F;
    public float shootInterval = 0F;

    private SpriteRenderer sr;
    private Sprite deactiveSprite;
    private float warmUpInterval;


    // Start is called before the first frame update
    void Start()
    {
        warmUpInterval = (shootInterval + shootTime) - warmUpTime;
        sr = GetComponent<SpriteRenderer>();
        deactiveSprite = sr.sprite;
        StartCoroutine(ShootRepeating());
    }
    

    IEnumerator ShootRepeating() {
        for (; ; ) {
            yield return new WaitForSeconds(warmUpInterval);
            sr.sprite = warmUpSprite;
            yield return new WaitForSeconds(warmUpTime);
            sr.sprite = shootSprite;
            yield return new WaitForSeconds(shootTime/2);
            ShootProjectile();
            yield return new WaitForSeconds(shootTime/2);
            sr.sprite = deactiveSprite;
        }
    }


    public void ShootProjectile() {
        float dirX = (sr.flipX ? -1 : 1);        
        Vector3 target = new Vector3(dirX, 0, 0);

        GameObject projectileGo = Instantiate(prefabProjectile, spawnPositionProjectile.transform.position, transform.rotation, EffectCollection.GetInstance().transform);
        projectileGo.GetComponent<Projectile>().InitProjectile(target, true);
    }

}
