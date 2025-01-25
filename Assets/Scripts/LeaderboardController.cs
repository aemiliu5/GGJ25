using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class LeaderboardController : MonoBehaviour {
        [SerializeField] private LeaderboardItem leaderboardItem;
        [SerializeField] private RectTransform leaderboardHolder;

        private PlayerDataProvider _playerDataProvider;
        private List<PlayerData> _playerData = new List<PlayerData>();

        private void OnEnable() {
           _playerDataProvider = PlayerDataProvider.Instance;

            // Load leaderboard entries
            var upcomingData = _playerDataProvider.GetAllPlayerData();

            if(_playerData.Count == upcomingData.Count) { return; }

            _playerData = upcomingData;
            _playerData.ForEach(data => Debug.Log(data.PlayerName));
            ConstructObject();
        }

        private void ConstructObject() {
            foreach (var entry in _playerData) {
                LeaderboardItem tempItem = Instantiate(leaderboardItem, leaderboardHolder);
                tempItem.InitItem(entry);
            }
        }
    }
}
