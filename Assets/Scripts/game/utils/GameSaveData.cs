using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public string[] collectableIds;
    public string[] activatedSavePoints;
    public float spawnPosX;
    public float spawnPosY;

    public Vector3 GetSpawnPosition() {
        Vector3 v = new Vector3(spawnPosX, spawnPosY, 0);
        return v;
    }

    public void SetSpawnPosition(Vector3 v) {
        spawnPosX = v.x;
        spawnPosY = v.y;
    }
}
