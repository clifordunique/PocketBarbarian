﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove {

    void Move(Vector2 moveAmount, bool standingOnPlatform = false);

    bool IsBelow();
    bool IsAbove();
}
