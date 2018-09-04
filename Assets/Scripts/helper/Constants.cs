using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    public static float PPU = 16;

    public static float WorldUnitsPerPixel() {
        return 1 / PPU;
    } 
}
