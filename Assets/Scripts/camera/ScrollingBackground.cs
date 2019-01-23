using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public float delay = 0F;
    public float backgroundSize;
    public float parallaxSpeed;
    public float viewzone = 10;
    public float constantSpeed = 0F;

    private Transform cameraTransform;
    private Transform[] layers;
    
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;



	// Use this for initialization
	void Start () {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            layers[i] = transform.GetChild(i);
        }
        leftIndex = 0;
        rightIndex = layers.Length - 1;
	}
	
	// Update is called from follow camera
	public void UpdateAfterCameraChanges () {
        if (Time.time > delay) {
            float deltaX = cameraTransform.position.x - lastCameraX;
            if (constantSpeed <= 0) {
                if (parallaxSpeed < 1) {
                    float diffX = (deltaX * parallaxSpeed);
                    transform.position += Vector3.right * diffX;
                } else {
                    transform.position = new Vector2(cameraTransform.position.x, transform.position.y);
                }
            } else {
                transform.position += Vector3.right * constantSpeed;
            }

            lastCameraX = cameraTransform.position.x;
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewzone)) {
                ScrollLeft();
            }

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewzone)) {
                ScrollRight();
            }
        }
    }


    private void ScrollLeft() {
        layers[rightIndex].position = new Vector3((layers[leftIndex].position.x - backgroundSize), layers[leftIndex].position.y, layers[leftIndex].position.z);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0) {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight() {
        layers[leftIndex].position = new Vector3((layers[rightIndex].position.x + backgroundSize), layers[rightIndex].position.y, layers[rightIndex].position.z);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length) {
            leftIndex = 0;
        }
    }
}
