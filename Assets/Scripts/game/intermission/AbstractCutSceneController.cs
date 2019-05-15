using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCutSceneController: MonoBehaviour {

    public bool finished = false;
    public bool playCutScene = false;

    public abstract void SkipScene();
}
