using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

    public SpriteRenderer lines;
    public SpriteRenderer company;
    public Animator fadeAnimator;
    public Image fadeImage;

    // Use this for initialization
    void Start () {
        fadeAnimator.SetBool("FADE_IN_SCENE", true);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey) {
            StartCoroutine(FadeOut());
        }

        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;
        lines.size = new Vector2(width, lines.size.y);
        SetGUIPosition(lines.gameObject, 0.5f, 0.0f, 0, 1);
        SetGUIPosition(company.gameObject, 0.0f, 0.0f, 3, 1);
    }


    private void SetGUIPosition(GameObject go, float x, float y, float offsetX, float offsetY) {
        float z = go.transform.position.z;
        go.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        go.transform.position = new Vector3(go.transform.position.x + offsetX, go.transform.position.y + offsetY, z);
    }


    IEnumerator FadeOut() {
        fadeAnimator.SetBool("FADE_OUT", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);

        LevelManager.GetInstance().LoadNextLevel();
    }
}
