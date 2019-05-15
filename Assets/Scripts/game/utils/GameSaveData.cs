using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class LevelDictionary: SerializableDictionaryBase<string, LevelSaveData> { }

[System.Serializable]
public class GameSaveData
{
    public string currentLevel;

    [SerializeField]
    public LevelDictionary levelDict = new LevelDictionary();

    public LevelSaveData GetCurrentLevelData() {
        return levelDict[currentLevel];
    }

    public LevelSaveData GetLevelDataFor(string level) {
        if (levelDict.ContainsKey(level)) {
            return levelDict[level];
        } else {
            return null;
        }
    }

    public void SetCurrentLevelData(LevelSaveData levelData) {
        levelDict[currentLevel] = levelData;
    }
}
