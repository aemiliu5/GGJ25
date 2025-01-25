using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveSystemWrapper : MonoBehaviour {
    [SerializeField] private string saveFileName;
    [SerializeField] private List<PlayerData> dummyData;

    private SaveJson _saveJson;

    private List<PlayerData> _playerData;

    private void Awake() {
        _saveJson = new SaveJson(saveFileName);
    }

    private IEnumerator Start() {
        //_saveJson.WriteData("playerData", dummyData);
        yield return new WaitForEndOfFrame();
        _playerData = (List<PlayerData>)_saveJson.ReadData("playerData");
        _playerData.ForEach(data => Debug.Log(data.PlayerName));
    }
}
