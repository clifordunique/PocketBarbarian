using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public MoveController2D target;
	public float verticalOffset;
    public float horizontalOffset;
    public float lookAheadDstX;
    public float lookAheadDstY;
    public float lookSmoothTimeX;
    public float lookSmoothTimeY;
    public float verticalSmoothTime;
	public Vector2 focusAreaSize;
    public Vector2 postInitOffsets;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
    
    float lookAheadDirY;
    float smoothVelocityY;
	bool lookAheadStopped;


    // Cam shake properties
    [Header("CamShake")]
    public float stompTime = 0.2F;
    public float stompIntensity = 0.5F;

    public float shakeSmallTime = 0.1F;
    public static float shakeSmallIntensity = 0.2F;

    public float shakeMediumTime = 0.3F;
    public static float shakeMediumIntensity = 0.5F;

    public float shakeBigTime = 0.4F;
    public static float shakeBigIntensity = 1F;

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
        currentLookAheadX = 0;
        targetLookAheadX = 0;
        lookAheadDirX = 0;
        smoothLookVelocityX = 0;

        lookAheadDirY = 0;
        smoothVelocityY = 0;
        lookAheadStopped = false;

        //transform.position = target.transform.position;
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
        verticalSmoothTimeTmp = verticalSmoothTime;
        scrollingBackgrounds = FindObjectsOfType<ScrollingBackground>();
        // Save x/y offsets for later camera movements
        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset + Vector2.right * horizontalOffset;
        postInitOffsets.x = focusPosition.x - target.transform.position.x;
        postInitOffsets.y = focusPosition.y - target.transform.position.y;
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
        this.shakeTime = Time.timeSinceLevelLoad + shakeTime;
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

        focusArea.Update (target, lookAheadDstY);

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

        float currentSmoothTime = verticalSmoothTime;
        if (!shake && target.collisions.below && InputController.GetInstance().GetDirectionLookaround() != 0) {
            currentSmoothTime = lookSmoothTimeY;
        }        
        if (shake) {
            currentSmoothTime = 0.0F;
        }

        focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, currentSmoothTime);        
		focusPosition += Vector2.right * currentLookAheadX;
        

        if (shake) {
            if (shakeTime > Time.timeSinceLevelLoad) {
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



    public IEnumerator MoveCameraBack(float cameraMoveSeconds, Vector2 startPos) {
        float t = 0.0f;
        Vector2 endPos = target.transform.position + Vector3.right * postInitOffsets.x + Vector3.up * postInitOffsets.y;

        while (t <= 1.0) {
            t += Time.deltaTime / cameraMoveSeconds;
            float v = t;
            v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            Vector3 newPos = new Vector3(pixelPerfectMoveAmount.x, pixelPerfectMoveAmount.y, Camera.main.transform.position.z);
            Camera.main.transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
        enabled = true;
        Init();
    }

    struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		public float left,right;
		public float top,bottom;


		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
		}

		public void Update(MoveController2D target, float lookAheadDstY) {
            Bounds targetBounds = target.myCollider.bounds;
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

            if (target.collisions.below && InputController.GetInstance().GetDirectionLookaround() != 0) {
                if (InputController.GetInstance().GetDirectionLookaround() > 0) {
                    // hoch gucken
                    shiftY = (targetBounds.max.y + lookAheadDstY + (top - bottom)) - top;
                } else {
                    // runter gucken
                    shiftY = (targetBounds.min.y - lookAheadDstY - (top - bottom)) - bottom;
                }
                
            }
			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}

}
