using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerNameField;
    [SerializeField] private TextMeshProUGUI playerScoreField;

    public void InitItem(PlayerData playerData) {
        playerNameField.SetText(playerData.PlayerName);
        playerScoreField.SetText(playerData.PlayerScore.ToString());
    }
}
