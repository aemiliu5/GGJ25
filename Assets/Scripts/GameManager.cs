using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

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
		loseCanvas.GetComponent<CanvasGroup>().alpha = 0;
		loseCanvas.GetComponent<CanvasGroup>().interactable = false;
		
		_sceneLoader = SceneLoader.Instance;

		Application.targetFrameRate = 120;
		PlayerDataProvider.Instance.ResetScore();
	}

	private void Update()
	{
		#if UNITY_ANDROID
		if (currentGameState == GameState.BEFORE_PLAY)
		{
			if (Input.touches.Length == 1)
			{
				StartGame();
			}
		}
		
		if (Input.touches.Length > 2)
		{
			_sceneLoader.LoadScene(SceneName.SampleScene);

		}
		#endif

		if (Input.GetKeyDown(KeyCode.F))
			Lose();
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
		loseCanvas.GetComponent<CanvasGroup>().DOFade(1f, 1f);
		loseCanvas.GetComponent<CanvasGroup>().interactable = true;
		MusicManager.instance.floriko.volume = 0f;
		MusicManager.instance.metal.volume = 0f;
		
		scoreText.text = $"Game Over!\nScore: {ScoreManager.instance.scoreText.text}";
		
		currentGameState = GameState.LOST;
		ScoreManager.instance.ApplyHighscore();
		AudioManager.instance.PlaySoundOnce(AudioManager.instance.loseMusic);
		//Time.timeScale = 0;
	}
}
