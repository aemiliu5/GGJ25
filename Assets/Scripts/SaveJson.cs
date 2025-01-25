using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SaveJson {
    private readonly string _fileName;
    private readonly string _savePath;
    private Dictionary<string, object> _saveData;
    public SaveJson(string saveFileName) {
        _fileName = saveFileName;
       _savePath = Application.persistentDataPath + _fileName;
        if (!FileExists(_savePath)) {
            Debug.Log("-- FILE DOESN'T EXIST - CREATING --");
        }

        _saveData = new Dictionary<string, object>();
    }

    // TODO:: Write Data to file
    public void WriteData(string key, object data) {
        using FileStream file = File.OpenWrite(_savePath);
        BinaryFormatter bf = new BinaryFormatter();
        _saveData[key] = data;
        bf.Serialize(file, _saveData);
        Debug.Log("-- SAVING CONTENT --");
    }

    private void LoadData() {
        using FileStream file = File.OpenRead(_savePath);
        BinaryFormatter bf = new BinaryFormatter();
        _saveData = (Dictionary<string, object>)bf.Deserialize(file);
    }

    // TODO:: Read Data from file
    public object ReadData(string key) {
        if(_saveData.Count == 0) {
            LoadData();
        }

        bool hasValue = _saveData.TryGetValue(key, out var obj);

        if(hasValue)
            Debug.Log($"-- FETCING ITEM OF TYPE : {nameof(obj.GetType)}");
        else
            Debug.Log($"-- COULD NOT FETCH ITEM --");

        return obj;
    }

    private bool FileExists(string filePath) {
        if (!File.Exists(filePath)) {
            File.Create(filePath);
            return false;
        }
        return true;
    }
}
