using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
	[FormerlySerializedAs("combo")] public int streak;
	public int score;
	public int highscore;

	public TextMeshProUGUI comboText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highscoreText;

	public static ScoreManager instance;

	private void Start()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;
		highscore = PlayerPrefs.GetInt("Highscore");
	}

	public void Update()
	{
		comboText.text = streak.ToString();
		scoreText.text = score.ToString();
		highscoreText.text = highscore.ToString();
		
		if (Input.GetKeyDown(KeyCode.F7))
			DeleteAllData();
	}

	public void AddStreak()
	{
		streak++;
	}

	public void ResetStreak()
	{
		streak = 0;
	}

	public void AddScore(int s)
	{
		score += s;
	}

	public void DeleteAllData()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}

	public void ApplyHighscore()
	{
		if (score > highscore)
			highscore = score;
		
		PlayerPrefs.SetInt("Highscore", highscore);
		PlayerPrefs.Save();
	}
}
