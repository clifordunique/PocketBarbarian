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
            foreach(string id in saveData.collectableIds) {
                Debug.Log("-> ID:" + id);
            }
            DeleteCollectables(saveData);
        }
    }

    private void DeleteCollectables(GameSaveData saveData) {
        AbstractCollectable[] currentCollectables = GetAllCollectables();
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
    

    public void Save() {
        lastSaveData = CreateSaveData();
        Thread _t1 = new Thread(SaveToFile);
        _t1.Start();        
    }

    private void SaveToFile() {
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("SavePath:" + path);
        FileStream file = File.Create(path);
        bf.Serialize(file, lastSaveData);
        file.Close();
        Debug.Log("Save complete");
    }

    private GameSaveData CreateSaveData() {
        GameSaveData saveData = new GameSaveData();

        // get collectables
        AbstractCollectable[] collectables = GetAllCollectables();
        Debug.Log("Found " + collectables.Length + " collectables to save...");
        saveData.collectableIds = new string[collectables.Length];
        for (int i = 0; i < collectables.Length; i++) {
            AbstractCollectable collectable = collectables[i];
            saveData.collectableIds[i] = GetUniqueId(collectable.gameObject);
        }
        return saveData;
    }

    private AbstractCollectable[] GetAllCollectables() {
        AbstractCollectable[] collectables = FindObjectsOfType<AbstractCollectable>();
        return collectables;
    }

    private string GetUniqueId(GameObject go) {
        UniqueId uid = go.GetComponent<UniqueId>();
        if (uid) {
            return uid.uniqueId;
        }
        return "";
    }
}
