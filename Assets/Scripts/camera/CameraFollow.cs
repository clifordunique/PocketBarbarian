using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public MoveController2D target;
	public float verticalOffset;
    public float horizontalOffset;
    public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	bool lookAheadStopped;


    // Cam shake properties
    [Header("CamShake")]
    public float stompTime = 0.2F;
    public float stompIntensity = 0.5F;

    public float shakeSmallTime = 0.2F;
    public float shakeSmallIntensity = 0.3F;

    public float shakeMediumTime = 0.3F;
    public float shakeMediumIntensity = 0.5F;

    public float shakeBigTime = 0.4F;
    public float shakeBigIntensity = 1F;

    bool shake = false;
    float shakeTime;
    float shakeIntensityX;
    float shakeIntensityY;
    bool down;

    float verticalSmoothTimeTmp;
    ScrollingBackground[] scrollingBackgrounds;

    static CameraFollow _instance;

    private void Awake() {
        _instance = this;
    }
    void Start() {        
        Init();
	}

    public void Init() {
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
        verticalSmoothTimeTmp = verticalSmoothTime;
        scrollingBackgrounds = FindObjectsOfType<ScrollingBackground>();
    }

    public static CameraFollow GetInstance() {
        if (_instance) {
            return _instance;
        } else {
            Debug.LogError("No instance of CameraFollow found!");
            return null;
        }
    }

    public void ShakeStamp() {
        ShakeCamera(stompTime, 0F, stompIntensity, true);
    }
    public void ShakeSmall() {
        ShakeCamera(shakeSmallTime, shakeSmallIntensity, shakeSmallIntensity);
    }
    public void ShakeMedium() {
        ShakeCamera(shakeMediumTime, shakeMediumIntensity, shakeMediumIntensity);
    }
    public void ShakeBig() {
        ShakeCamera(shakeBigTime, shakeBigIntensity, shakeBigIntensity);
    }

    public void ShakeCamera (float shakeTime, float shakeIntensityX, float shakeIntensityY, bool down = false) {
        shake = true;
        this.shakeTime = Time.time + shakeTime;
        this.shakeIntensityX = shakeIntensityX;
        this.shakeIntensityY = shakeIntensityY;
        this.down = down;
    }

    public void CheckForPlayerOnPlatform(Transform passenger) {
        if (target.transform == passenger.transform) {
            verticalSmoothTime = 0;
        }
    }

	void LateUpdate() {
		focusArea.Update (target.myCollider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset + Vector2.right * horizontalOffset;
        

        if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign(target.moveDirectionX) == Mathf.Sign(focusArea.velocity.x)) {// && target.moveDirectionY != 0) {
				lookAheadStopped = false;                
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            }
			else {
				if (!lookAheadStopped) {
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX)/4f;
				}
			}
		}


		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, (shake ? 0.0F : verticalSmoothTime));
        //Debug.Log("CurrentLookAheadX:" + currentLookAheadX);
		focusPosition += Vector2.right * currentLookAheadX;

        if (shake) {
            if (shakeTime > Time.time) {
                float modifierRight = Random.Range(-shakeIntensityX, shakeIntensityX);
                float modifierUp;
                if (down) {
                    modifierUp = Random.Range(shakeIntensityY, 0);
                } else {
                    modifierUp = Random.Range(-shakeIntensityY, shakeIntensityY);
                }
                focusPosition = (Vector3)focusPosition + Vector3.up * modifierUp + Vector3.right * modifierRight;
            } else {
                shake = false;
            }
        }
        Vector2 pixelPerfectFocus = Utils.MakePixelPerfect(focusPosition);
        transform.position = (Vector3)pixelPerfectFocus + Vector3.forward * (-10);

        verticalSmoothTime = verticalSmoothTimeTmp;

        if (scrollingBackgrounds != null && scrollingBackgrounds.Length > 0) {
            foreach (ScrollingBackground scrollingBackground in scrollingBackgrounds) {
                scrollingBackground.UpdateAfterCameraChanges();
            }
        }
    }

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.centre, focusAreaSize);
	}

	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		float left,right;
		float top,bottom;


		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}

}
