using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    public static string SORTING_LAYER_MASKED_SPRITES = "MaskedSprites";
    public static string SORTING_LAYER_Player = "Player";
    public static string SORTING_LAYER_DIALOGUE = "Dialogue";
    public static string SORTING_LAYER_GUI = "GUI";
    

    public static float PPU = 16;

    public static float WorldUnitsPerPixel() {
        return 1 / PPU;
    } 
}
