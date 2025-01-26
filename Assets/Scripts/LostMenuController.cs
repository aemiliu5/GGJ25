
using System;
using System.Collections.Generic;
using UnityEngine;

public class LostMenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    
    private SceneLoader _sceneLoader;

    private void Start()
    {
        _sceneLoader = SceneLoader.Instance;
    }

    public void LoadMainMenuScene()
    {
        _sceneLoader.LoadScene(SceneName.MainMenu);
    }

    public void ReloadGame()
    {
        _sceneLoader.LoadScene(SceneName.SampleScene);
    }
    
    public void ChangePanelState(int mainMenuState) 
    {
        for (int i = 0; i < panels.Count; i++) {
            panels[i].SetActive(i == mainMenuState);
        }
    }
}
