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
    bool shake = false;
    float shakeTime;
    float shakeIntensityX;
    float shakeIntensityY;
    bool down;

    static CameraFollow _instance;

    void Start() {
        _instance = this;
        Init();
	}

    public void Init() {
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
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
        ShakeCamera(0.15F, 0F, 0.2F, true);
    }
    public void ShakeSmall() {
        ShakeCamera(0.1F, 0.1F, 0.1F);
    }
    public void ShakeBig() {
        ShakeCamera(0.2F, 0.3F, 0.3F);
    }

    public void ShakeCamera (float shakeTime, float shakeIntensityX, float shakeIntensityY, bool down = false) {
        shake = true;
        this.shakeTime = Time.time + shakeTime;
        this.shakeIntensityX = shakeIntensityX;
        this.shakeIntensityY = shakeIntensityY;
        this.down = down;
    }

	void LateUpdate() {
		focusArea.Update (target.myCollider.bounds);

        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset + Vector2.right * horizontalOffset;
        

        if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);
			if (Mathf.Sign(target.moveDirectionX) == Mathf.Sign(focusArea.velocity.x) && target.moveDirectionY != 0) {
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

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;

        if (shake) {
            if (shakeTime > Time.time) {
                float modifierRight = Random.Range(-shakeIntensityX, shakeIntensityX);
                float modifierUp;
                if (down) {
                    modifierUp = Random.Range(shakeIntensityY, 0);
                    Debug.Log(modifierUp);
                } else {
                    modifierUp = Random.Range(-shakeIntensityY, shakeIntensityY);
                }
                Vector3 shakePosition = (Vector3)focusPosition + Vector3.forward * (-10) + Vector3.up * modifierUp + Vector3.right * modifierRight;
                transform.position = Utils.MakePixelPerfect(shakePosition);
            } else {
                shake = false;
            }
        } else {
            // Round to pixelPerfect
            
            Vector2 pixelPerfectFocus = Utils.MakePixelPerfect(focusPosition);
            transform.position = (Vector3)pixelPerfectFocus + Vector3.forward * (-10);
            
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
