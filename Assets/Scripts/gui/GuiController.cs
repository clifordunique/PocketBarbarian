using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour {

    public GameObject backgroundLeft;
    public GameObject backgroundCenter;
    public GameObject backgroundRight;

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        RefreshPositions();
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
