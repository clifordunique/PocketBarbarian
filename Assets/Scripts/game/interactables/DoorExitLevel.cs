using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExitLevel : Door
{
    public override void Activate() {
        OpenDoor();
        actionFinished = true;
        actionArrow.SetActive(false);
    }
}
