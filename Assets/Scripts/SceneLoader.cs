using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {
    public static SceneLoader Instance { get; private set; }
    public void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(SceneName sceneName) {
        string scName = sceneName.ToString();
        SceneManager.LoadScene(scName);
    }
}