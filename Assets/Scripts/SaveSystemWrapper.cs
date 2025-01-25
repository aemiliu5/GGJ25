using UnityEngine;

public class SaveSystemWrapper : MonoBehaviour {
    public static SaveSystemWrapper Instance { get; private set; }

    [SerializeField] private string saveFileName;

    private SaveJson _saveJson;


    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _saveJson = new SaveJson(saveFileName);
    }

    public object ReadData(string key) {
        return _saveJson.ReadData(key);
    }

    public void WriteData(string key, object data) {
        _saveJson.WriteData(key, data);
    }
}
