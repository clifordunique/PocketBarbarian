using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenueItemSprite
{
    void SetEnabled();
    void SetDisabled();
    void Click();
    float GetWidth();
}
