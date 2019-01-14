using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

public class LoadSaveGame: MonoBehaviour
{
    private GameSaveData lastSaveData;
    private string path;

    public void Awake() {
        path = Application.persistentDataPath + "/savedGames.gd";
    }


    public void Load() {
        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            GameSaveData saveData = (GameSaveData)bf.Deserialize(file);
            file.Close();

            
            ActivateSavePoints(saveData);
            DeleteCollectables(saveData);

            SetSpawnPosition(saveData);
        }
    }

    private void SetSpawnPosition(GameSaveData saveData) {
        PlayerController.GetInstance().transform.position = saveData.GetSpawnPosition();
    }

    private void ActivateSavePoints(GameSaveData saveData) {
        SavePoint[] currentSavePoints = FindObjectsOfType<SavePoint>();
        foreach (SavePoint currentSavePoint in currentSavePoints) {
            string uniqueId = GetUniqueId(currentSavePoint.gameObject);
            if (uniqueId != "") {
                if (Array.IndexOf(saveData.activatedSavePoints, uniqueId) != -1) {
                    // found, so activate this item
                    currentSavePoint.SetActive(true);
                }
            }
        }
    }

    private void DeleteCollectables(GameSaveData saveData) {
        AbstractCollectable[] currentCollectables = FindObjectsOfType<AbstractCollectable>(); 
        foreach(AbstractCollectable currentCollectable in currentCollectables) {
            string uniqueId = GetUniqueId(currentCollectable.gameObject);
            if (uniqueId != "") {
                if (Array.IndexOf(saveData.collectableIds, uniqueId) == -1) {
                    // not found, so delete this item
                    Destroy(currentCollectable.gameObject);
                }                
            }
        }
    }
    

    public void Save(SavePoint savePoint) {
        lastSaveData = CreateSaveData(savePoint);
        Thread _t1 = new Thread(SaveToFile);
        _t1.Start();        
    }

    private void SaveToFile() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, lastSaveData);
        file.Close();
        Debug.Log("Save Game complete");
    }

    private GameSaveData CreateSaveData(SavePoint savePoint) {
        GameSaveData saveData = new GameSaveData();

        // save spawnPoint
        saveData.SetSpawnPosition(savePoint.GetSpawnPosition());

        // save active SavePoints
        saveData.activatedSavePoints = GetAllActiveSavePointsUniqueIds();

        // get collectables
        saveData.collectableIds = GetAllCollectablesUniqueIds();


        return saveData;
    }


    private string[] GetAllActiveSavePointsUniqueIds() {
        SavePoint[] savePoints = FindObjectsOfType<SavePoint>();
        Debug.Log("Found " + savePoints.Length + " savePoints to save...");
        if (savePoints != null) {
            List<string> result = new List<string>();
            for (int i = 0; i < savePoints.Length; i++) {
                SavePoint savePoint = savePoints[i];
                if (savePoint.activated) {
                    result.Add(GetUniqueId(savePoint.gameObject));
                }
            }
            return result.ToArray();
        }
        return new string[0];
    }

    private string[] GetAllCollectablesUniqueIds() {
        AbstractCollectable[] collectables = FindObjectsOfType<AbstractCollectable>();
        Debug.Log("Found " + collectables.Length + " collectables to save...");
        if (collectables != null) {
            string[] result = new string[collectables.Length];
            for (int i = 0; i < collectables.Length; i++) {
                AbstractCollectable collectable = collectables[i];
                result[i] = GetUniqueId(collectable.gameObject);
            }
            return result;
        }
        return new string[0];
    }

    private string GetUniqueId(GameObject go) {
        UniqueId uid = go.GetComponent<UniqueId>();
        if (uid) {
            return uid.uniqueId;
        }
        return "";
    }
}
