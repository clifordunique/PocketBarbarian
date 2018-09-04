using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MoveGroundController2D))]
public class PlayerInput: StateMachineGameActor {

    MoveGroundController2D player;
    

    public override void Start() {
        base.Start();
        player = GetComponent<MoveGroundController2D>();
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
            // CameraFollow.GetInstance().ShakeBig();
            //bezierenemyTest.StartFlight(transform);
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Input.GetKeyDown(KeyCode.X)) {
            player.OnDash();
        }
    }

    
}
