using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractActivatable : MonoBehaviour {

    public abstract bool Activate();
    public abstract bool DeActivate();
}
