using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActorController {

    void ReactHurt(bool dead, bool push, Vector3 hitSource, HitBox.DAMAGE_TYPE damageType);
    void ReactHit();
}
