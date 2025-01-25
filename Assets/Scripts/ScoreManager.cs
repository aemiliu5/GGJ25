using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour {
	[FormerlySerializedAs("combo")] public int streak;

	public TextMeshProUGUI comboText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highscoreText;

	public static ScoreManager instance;

	private PlayerDataProvider _playerDataProvider;
	private int HighScore => _playerDataProvider != null ? _playerDataProvider.HighScore : 0;
	private int Score => _playerDataProvider != null ? _playerDataProvider.CurrentScore : 0;

	private void Start()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;
		_playerDataProvider = PlayerDataProvider.Instance;
	}

	public void Update() {
		if (_playerDataProvider == null) return;

		comboText.text = $"{streak}";
		scoreText.text = $"{Score}";
		highscoreText.text = $"{HighScore}";
		
		if (Input.GetKeyDown(KeyCode.F7))
			DeleteAllData();
	}

	public void AddStreak() {
		streak++;
	}

	public void ResetStreak()
	{
		streak = 0;
	}

	public void AddScore(int s) {
		_playerDataProvider.IncreasePlayerScore(s); 
	}

	public void DeleteAllData() {
		_playerDataProvider.ResetScore();
	}

	public void ApplyHighscore() {
		_playerDataProvider.SaveCurrentPlayerData();
	}
}
