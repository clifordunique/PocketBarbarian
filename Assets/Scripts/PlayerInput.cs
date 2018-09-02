using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerInput: MonoBehaviour {

    CharacterController2D player;
    public LayerMask enemyAttackLayer;

    void Start() {
        player = GetComponent<CharacterController2D>();
    }

    void Update() {
        player.OnMove(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space)) {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            player.OnJumpInputUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            player.OnStamp();
            CameraFollow.GetInstance().ShakeBig();
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Input.GetKeyDown(KeyCode.X)) {
            player.OnDash();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) {

        // react to attacks
        if (enemyAttackLayer == (enemyAttackLayer | (1 << collision.gameObject.layer))) {
            Vector2 hitDirections = GetHitDirection(collision);

            if (hitDirections.y > 0) {
                player.OnPush(hitDirections.x, hitDirections.y, 15, 0.05F);
            } else {
                player.OnPush(hitDirections.x, hitDirections.y, 20, 0.05F);
            }
        }
    }

    public Vector2 GetHitDirection(Collision2D collision) {
        Vector3 v = new Vector3(transform.position.x - collision.transform.position.x, transform.position.y - collision.transform.position.y, 1).normalized;
        Vector2 result = new Vector2();
        if (v.x > 0F) result.x = 1;
        if (v.x < -0F) result.x = -1;

        if (v.y > 0.5F) result.y = 1;
        if (v.y < -0.5F) result.y = -1;
        return result;
    }
    
}
