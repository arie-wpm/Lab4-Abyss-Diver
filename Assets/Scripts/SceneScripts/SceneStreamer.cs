using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneStreamer : MonoBehaviour
{
    public static SceneStreamer Instance;

    void Awake() {
        Instance = this;
    }

    void Start() {
        StartCoroutine(Startup());
    }

    IEnumerator Startup() {
        string startScene = GameManager.instance.startLevel;
        GameManager.currentScene = startScene;
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);
    }

    public void LoadSceneByName(string sceneName)
    {
        GameManager.previousScene = GameManager.currentScene;
        GameManager.currentScene = sceneName;
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void UnloadSceneByName(string sceneName)
    {
        StartCoroutine(UnloadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!op.isDone) yield return null;
    }

    private IEnumerator UnloadSceneRoutine(string sceneName)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
        while (!op.isDone) yield return null;
    }
}