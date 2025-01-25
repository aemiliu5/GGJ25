using UnityEngine;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private List<GameObject> panels;

    private SceneLoader _sceneLoader;
    private void Start() {
        _sceneLoader = SceneLoader.Instance;    
    }

    public void LoadGameplayScene() {
        _sceneLoader.LoadScene(SceneName.SampleScene);
    }

    public void CloseLeaderboard() {
        panels[1].SetActive(false);
        panels[0].SetActive(true);
    }

    public void OpenLeaderboard() {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
