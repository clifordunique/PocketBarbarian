using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiLevelCompleteController : MonoBehaviour
{
    public float delayTimeText;
    public GameObject textObj;


    // Use this for initialization
    void Start() {
        transform.position = GetPosition(0.5F, 0.5F, 0);
        StartCoroutine(EnableText());
    }

    private Vector3 GetPosition(float x, float y, float offsetY) {
        Vector3 result;
        float z = transform.position.z;
        result = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        result = new Vector3(result.x, result.y + offsetY, z);
        return result;
    }

    IEnumerator EnableText() {
        yield return new WaitForSeconds(delayTimeText);
        textObj.SetActive(true);
    }
}
