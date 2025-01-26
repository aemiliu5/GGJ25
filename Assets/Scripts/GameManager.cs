using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject loseCanvas;
	[SerializeField] private TextMeshProUGUI scoreText;
	
	public enum GameState
	{
		BEFORE_PLAY,
		PLAY,
		PAUSE,
		LOST
	}

	public static GameManager instance;
	private SceneLoader _sceneLoader;


	public void Start()
	{
		if(instance != null) 
			Destroy(instance.gameObject);
		
		instance = this;
		ChangeGameState(GameState.BEFORE_PLAY);
		loseCanvas.SetActive(false);
		
		_sceneLoader = SceneLoader.Instance;

		Application.targetFrameRate = 120;
	}

	private void Update()
	{
		if (currentGameState == GameState.BEFORE_PLAY)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartGame();
			}
		}

		// Restart
		if (Input.GetKeyDown(KeyCode.R)) {
			_sceneLoader.LoadScene(SceneName.SampleScene);
		}
	}

	public GameState currentGameState;

	public void ChangeGameState(GameState newGameState)
	{
		currentGameState = newGameState;
		//Time.timeScale = (currentGameState == GameState.PLAY || currentGameState == GameState.BEFORE_PLAY) ? 1 : 0;

		if (currentGameState == GameState.LOST) {
            Lose();
        }
    }

	public void StartGame()
	{
		ChangeGameState(GameState.PLAY);
		Time.timeScale = 1;
		PlayerController.instance.GetComponent<Rigidbody2D>().gravityScale = 1;
		PlayerController.instance.InitialJump();
	}

	public void Lose()
	{
		loseCanvas.SetActive(true);
		scoreText.text = $"Game Over!\nScore: {ScoreManager.instance.scoreText.text}";
		
		currentGameState = GameState.LOST;
		ScoreManager.instance.ApplyHighscore();
		MusicManager.instance.floriko.volume = 0f;
		MusicManager.instance.metal.volume = 0f;
		AudioManager.instance.PlaySoundOnce(AudioManager.instance.loseMusic);
		//Time.timeScale = 0;
	}
}
