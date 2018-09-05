using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    [Header("Damage")]
    // damage on contact
    public int damage;

    public enum DAMAGE_TYPE { DEFAULT};
    public DAMAGE_TYPE damageType;
}
