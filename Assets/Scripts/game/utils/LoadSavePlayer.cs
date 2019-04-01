using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

public class LoadSavePlayer : MonoBehaviour
{
    private PlayerSaveData lastSaveData;
    private string path;

    public void Awake() {
        path = Application.persistentDataPath + "/savedPlayer.gd";
    }

    public void Delete() {
        // Temporary
        if (File.Exists(path)) {
            File.Delete(path);
        }
    }


    public void Load() {
        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerSaveData saveData = (PlayerSaveData)bf.Deserialize(file);
            file.Close();

            PlayerController.GetInstance().statistics.RefreshSaveData(saveData);
        }
    }



    public void Save() {
        lastSaveData = PlayerController.GetInstance().statistics.CreateSaveData();
        Thread _t1 = new Thread(SaveToFile);
        _t1.Start();
    }

    private void SaveToFile() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, lastSaveData);
        file.Close();
        Debug.Log("Save Player complete: " + path);
    }
}
