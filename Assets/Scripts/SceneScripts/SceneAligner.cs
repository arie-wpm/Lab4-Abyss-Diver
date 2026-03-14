using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneAligner : MonoBehaviour {
    private SceneReference sceneToLoad;
    private SceneReference Level1;
    private SceneReference Level2;
    private SceneReference Level3;
    private SceneReference Rest;

    [Header("Alignment")]
    public Transform previousSceneAnchor;

    private string newSceneAnchorName;

    private static HashSet<string> loadedScenes = new HashSet<string>();

    void Awake() {
        if (SceneManager.GetActiveScene().name == "Main") loadedScenes.Clear();
        Level1 = new SceneReference();
        Level2 = new SceneReference();
        Level3 = new SceneReference();
        Rest = new SceneReference();
        newSceneAnchorName = "RestEntry";

        Level1.SceneName = "Level1";
        Level2.SceneName = "Level2";
        Level3.SceneName = "Level3";
        Rest.SceneName = "Rest";

        sceneToLoad = Rest;
        GameManager.instance.currentSpawnPoint = GameObject.Find("SpawnPoint").transform;

        if (GameManager.currentScene == "Rest") {
            switch (GameManager.previousScene)
            {
                case "Level1":
                    sceneToLoad = Level2;
                    newSceneAnchorName = "Level2Entry";
                    break;
                case "Level2":
                    sceneToLoad = Level3;
                    newSceneAnchorName = "Level3Entry";
                    break;
                default:
                    Debug.LogWarning("Rest scene does not know previous scene!");
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.enabled = false;            

        if (sceneToLoad.SceneName == "Rest") {
            StartCoroutine(ReloadRestScene());
            return;
        }

        switch (newSceneAnchorName) {
            case "Level2Entry":
                SceneStreamer.Instance.UnloadSceneByName("Level1");
                break;
            case "Level3Entry":
                SceneStreamer.Instance.UnloadSceneByName("Level2");
                break;
            default:
                break;
        }

        if (!loadedScenes.Contains(sceneToLoad.SceneName)) {
            loadedScenes.Add(sceneToLoad.SceneName);
            SceneStreamer.Instance.LoadSceneByName(sceneToLoad.SceneName);
            StartCoroutine(AlignAfterLoad());
        }
    }

    private IEnumerator ReloadRestScene() {
        Scene restScene = SceneManager.GetSceneByName("Rest");
        if (restScene.isLoaded) {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(restScene);
            while (!unloadOp.isDone) yield return null;
        }

        SceneStreamer.Instance.LoadSceneByName("Rest");
        yield return StartCoroutine(AlignAfterLoad());
    }

    private IEnumerator AlignAfterLoad() {
        Scene loadedScene = SceneManager.GetSceneByName(sceneToLoad.SceneName);
        while (!loadedScene.isLoaded) yield return null;
        yield return null;

        GameObject newSceneAnchor = GameObject.Find(newSceneAnchorName);
        if (newSceneAnchor == null || previousSceneAnchor == null) {
            Debug.LogWarning("Scene alignment failed: missing anchors.");
            Debug.Log("new Anchor: " + newSceneAnchorName);
            yield break;
        }

        Vector3 offset = previousSceneAnchor.position - newSceneAnchor.transform.position;
        foreach (GameObject rootObj in loadedScene.GetRootGameObjects()) rootObj.transform.position += offset;

        // set spawn
        GameManager.instance.currentSpawnPoint = GameObject.Find("SpawnPoint").transform;
    }
}