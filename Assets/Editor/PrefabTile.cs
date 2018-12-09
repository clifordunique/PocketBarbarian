using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Prefab Tile", menuName = "Tiles/Prefab Tile")]
public class PrefabTile: UnityEngine.Tilemaps.TileBase {
    public Sprite Sprite; //The sprite of tile in the palette
    public GameObject Prefab; //The gameobject to spawn


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        // Assign variables
        if (!Application.isPlaying) tileData.sprite = Sprite;
        else tileData.sprite = null;

        if (Prefab) tileData.gameObject = Prefab;
    }

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go) {
        // Streangly the position of gameobject starts at Left Bottom point of cell and not at it center
        // TODO need to add anchor points  (vertical and horisontal (left,centre,right)(top,centre,bottom))
        go.transform.position = location + new Vector3(0.5f, 0.5f, 0);
        //go.transform.position += Vector3.up * 0.5f + Vector3.right * 0.5f;
        return true;
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData) {
        // Make sprite of tile invisiable
        tileAnimationData.animatedSprites = new Sprite[] { null };
        tileAnimationData.animationSpeed = 0;
        tileAnimationData.animationStartTime = 0;
        return true;
    }

}