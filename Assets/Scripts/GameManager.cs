using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
		Time.timeScale = (currentGameState == GameState.PLAY || currentGameState == GameState.BEFORE_PLAY) ? 1 : 0;
		PlayerController.instance.GetComponent<Rigidbody2D>().gravityScale = (currentGameState == GameState.PLAY) ? 1 : 0;
	}

	public void StartGame()
	{
		ChangeGameState(GameState.PLAY);
		Time.timeScale = 1;
		PlayerController.instance.InitialJump();
	}

	public void Lose()
	{
		// show losing UI
		currentGameState = GameState.LOST;
		ScoreManager.instance.ApplyHighscore();
		Time.timeScale = 0;
	}

}
