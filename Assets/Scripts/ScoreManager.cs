using DG.Tweening;
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

	private int _lastCheckedStreak; // New field to track the previous streak value

	private void Start() {
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;
		_playerDataProvider = PlayerDataProvider.Instance;

		_lastCheckedStreak = 0; // Initialize the last checked streak
	}

	public void Update() {
		if (_playerDataProvider == null) return;

		comboText.text = $"{streak}";
		scoreText.text = $"{Score}";
		highscoreText.text = $"{HighScore}";

		// Trigger Boost Mode only when streak transitions to a multiple of 5
		if (streak % 12 == 0 && streak > 0 && streak != _lastCheckedStreak) {
			PlayerController.instance.ActivateBoostMode();
			_lastCheckedStreak = streak; // Update the last checked streak
		}

		if (Input.GetKeyDown(KeyCode.F7))
			DeleteAllData();
	}

	public void AddStreak() {
		streak++;
		comboText.rectTransform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => comboText.rectTransform.DOScale(Vector3.one, 0.1f));
	}

	public void ResetStreak() {
		streak = 0;
		_lastCheckedStreak = 0; // Reset last checked streak to avoid triggering Boost Mode incorrectly
		comboText.rectTransform.DOShakePosition(0.25f, new Vector3(15f, 0, 0)).SetEase(Ease.InOutSine);
		comboText.DOColor(Color.red, 0.1f).OnComplete(() => comboText.DOColor(Color.white, 0.1f));

	}

	public void AddScore(int s) {
		_playerDataProvider.IncreasePlayerScore(s);
		scoreText.rectTransform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() => scoreText.rectTransform.DOScale(Vector3.one, 0.1f));

	}

	public void DeleteAllData() {
		_playerDataProvider.ResetScore();
	}

	public void ApplyHighscore() {
		_playerDataProvider.SaveCurrentPlayerData();
	}
}