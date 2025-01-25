using UnityEngine;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private List<GameObject> panels;

    private SceneLoader _sceneLoader;
    private PlayerDataProvider _playerDataProvider;
    private void Start() {
        _sceneLoader = SceneLoader.Instance;
        _playerDataProvider = PlayerDataProvider.Instance;
        _playerDataProvider.CreateNewPlayerData();
    }

    public void LoadGameplayScene() {
        _sceneLoader.LoadScene(SceneName.SampleScene);
    }

    public void ChangePanelState(int mainMenuState) {
        for (int i = 0; i < panels.Count; i++) {
            panels[i].SetActive(i == mainMenuState);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
