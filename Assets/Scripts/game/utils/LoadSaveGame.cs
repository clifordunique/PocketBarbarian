using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSaveGame: MonoBehaviour
{
    private GameSaveData lastSaveData = new GameSaveData();
    private string path;

    public void Awake() {
        path = Application.persistentDataPath + "/savedGames.gd";
    }



    //--------------------------------------------------------------------------------------------------
    //-- LOAD
    //--------------------------------------------------------------------------------------------------
    public void Load() {
        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            lastSaveData = (GameSaveData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Current Level:" + lastSaveData.currentLevel);
            string level = SceneManager.GetActiveScene().name;
            LevelSaveData levelData = lastSaveData.GetLevelDataFor(level);
            if (levelData == null) {
                Debug.Log("Level Data null");
            } else {
                Debug.Log("Level Data found!");
                ActivateSavePoints(levelData);
                DeleteCollectables(levelData);
                SetSpawnPosition(levelData);
                SkipCutScenes(levelData);
            }
        }
    }



    private void ActivateSavePoints(LevelSaveData saveData) {
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

    private void DeleteCollectables(LevelSaveData saveData) {
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

    private void SetSpawnPosition(LevelSaveData saveData) {
        PlayerController.GetInstance().transform.position = saveData.GetSpawnPosition();
    }


    private void SkipCutScenes(LevelSaveData saveData) {
        AbstractCutSceneController[] cutSceneController = FindObjectsOfType<AbstractCutSceneController>();
        foreach (AbstractCutSceneController currentCutScene in cutSceneController) {
            string uniqueId = GetUniqueId(currentCutScene.gameObject);
            if (uniqueId != "") {
                if (Array.IndexOf(saveData.finishedCutScenes, uniqueId) != -1) {
                    // found, so skip this scene
                    currentCutScene.SkipScene();
                }
            }
        }
    }




    //--------------------------------------------------------------------------------------------------
    //-- SAVE
    //--------------------------------------------------------------------------------------------------

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

        LevelSaveData levelData = new LevelSaveData();
        // save spawnPoint
        levelData.SetSpawnPosition(savePoint.GetSpawnPosition());

        // save active SavePoints
        levelData.activatedSavePoints = GetAllActiveSavePointsUniqueIds();

        // get collectables
        levelData.collectableIds = GetAllCollectablesUniqueIds();

        // get cutscenes
        levelData.finishedCutScenes = GetAllFinishedCutScenesUniqueIds();

        lastSaveData.currentLevel = SceneManager.GetActiveScene().name;
        lastSaveData.SetCurrentLevelData(levelData);
        return lastSaveData;
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


    private string[] GetAllFinishedCutScenesUniqueIds() {
        AbstractCutSceneController[] cutScenes = FindObjectsOfType<AbstractCutSceneController>();
        Debug.Log("Found " + cutScenes.Length + " cutscenes to save...");
        if (cutScenes != null) {
            List<string> result = new List<string>();
            for (int i = 0; i < cutScenes.Length; i++) {
                AbstractCutSceneController cutScene = cutScenes[i];
                if (cutScene.finished) {
                    result.Add(GetUniqueId(cutScene.gameObject));
                }
            }
            return result.ToArray();
        }
        return new string[0];
    }



    //--------------------------------------------------------------------------------------------------
    //-- UTILS
    //--------------------------------------------------------------------------------------------------


    private string GetUniqueId(GameObject go) {
        UniqueId uid = go.GetComponent<UniqueId>();
        if (uid) {
            return uid.uniqueId;
        }
        return "";
    }
}
