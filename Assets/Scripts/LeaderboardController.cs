using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts {
    public class LeaderboardController : MonoBehaviour {
        [SerializeField] private LeaderboardItem leaderboardItem;
        [SerializeField] private RectTransform leaderboardHolder;

        private PlayerDataProvider _playerDataProvider;
        private List<PlayerData> _playerData = new List<PlayerData>();

        private IEnumerator Start() {
            yield return new WaitForEndOfFrame();

           _playerDataProvider = PlayerDataProvider.Instance;

            // Load leaderboard entries
            var upcomingData = _playerDataProvider.GetAllPlayerData();

            if(_playerData.Count == upcomingData.Count) { yield break; }

            _playerData = upcomingData;
            _playerData.ForEach(data => Debug.Log(data.PlayerName));
            ConstructObject();
        }

        private void ConstructObject() {
            var sortedList = _playerData.OrderByDescending(item => item.PlayerScore);

            foreach (var entry in sortedList) {
                LeaderboardItem tempItem = Instantiate(leaderboardItem, leaderboardHolder);
                tempItem.InitItem(entry);
            }
        }
    }
}
