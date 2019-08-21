using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleEffectInstantiator: MonoBehaviour {
    public List<GameObject> prefabEffects;
    public float randomRadius;
    public float timer;

    private int currentPosition = 0;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(InstantiateRandom());
    }


    private IEnumerator InstantiateRandom() {
        if (prefabEffects.Count > currentPosition) {
            float randomXOffset = Random.Range(-1 * randomRadius, randomRadius);
            float randomYOffset = Random.Range(-1 * randomRadius, randomRadius);

            Vector2 position = new Vector2(transform.position.x + randomXOffset, transform.position.y + randomYOffset);

            InstantiateEffect(prefabEffects[currentPosition], position);
            currentPosition++;
            yield return new WaitForSeconds(timer);
            Debug.Log("New routine");
            StartCoroutine(InstantiateRandom());
        } else {
            Destroy(gameObject);
        }
    }

    public void InstantiateEffect(GameObject effectToInstanciate, Vector2 position) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = position;
    }
}
