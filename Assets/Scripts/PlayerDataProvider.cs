using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataProvider : MonoBehaviour {
    public static PlayerDataProvider Instance { get; private set; }
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private SaveSystemWrapper _saveSystemWrapper;
    private List<PlayerData> _allData = new List<PlayerData>();
    private PlayerData _currentPlayerData;

    public int HighScore { get; private set; }

    private void Start() {
        _saveSystemWrapper = SaveSystemWrapper.Instance;
    }

    public void CreateNewPlayerData() {
        _currentPlayerData = new PlayerData();
    }

    public void SetPlayerName(string playerName) {
        _currentPlayerData.PlayerName = playerName;
    }
    public void ResetScore() => _currentPlayerData.PlayerScore = 0;
    public void IncreasePlayerScore(int amount) => _currentPlayerData.PlayerScore += amount;
    public void DecreasePlayerScore(int amount) => _currentPlayerData.PlayerScore -= amount;
    public int CurrentScore => _currentPlayerData.PlayerScore;
    public List<PlayerData> GetAllPlayerData() {
        var tempList = (List<PlayerData>)_saveSystemWrapper.ReadData("playerData");
        if (_allData.Count != tempList.Count) {
            _allData = tempList;
        }

        return _allData;
    }

    public void SaveCurrentPlayerData() {
        _allData.Add(_currentPlayerData);
        _saveSystemWrapper.WriteData("playerData", _allData);

        HighScore = _allData.Max(item => item.PlayerScore);
    }
}
