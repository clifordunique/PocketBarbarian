using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour {

    public GameObject backgroundLeft;
    public GameObject backgroundCenter;
    public GameObject backgroundRight;
    public GameObject prefabDiedEffect;

    private static GuiController _instance;
    private float lastScreenWidth = 0;

    // Use this for initialization
    void Start () {
        _instance = this;
        Invoke("RefreshPositions", 0.5F);
    }

    public static GuiController GetInstance() {
        if (_instance) {
            return _instance;
        } else {
            Debug.LogError("GuiController not yet instanciated!");
            return null;
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (Screen.width != lastScreenWidth) {
            RefreshPositions();
            lastScreenWidth = Screen.width;
        }
    }

    public void InstanciateDiedEffect() {
        GameObject effect = Instantiate(prefabDiedEffect);
        effect.transform.parent = EffectCollection.GetInstance().transform;
    }

    public void RefreshPositions() {
        SetGUIPosition(backgroundLeft, 0f, 1.0f, 0, 0);
        SetGUIPosition(backgroundCenter, 0.5f, 1.0f, (5F / Constants.PPU), 0);
        SetGUIPosition(backgroundRight, 1f, 1.0f, 0, 0);
    }

    private void SetGUIPosition(GameObject go, float x, float y, float offsetX, float offsetY) {
        float z = go.transform.position.z;
        go.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        go.transform.position = new Vector3(go.transform.position.x + offsetX, go.transform.position.y + offsetY, z);
    }
}
