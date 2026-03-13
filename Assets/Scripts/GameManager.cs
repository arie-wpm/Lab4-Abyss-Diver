using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Test Flags")]
    public bool enableGodMode = false;


    // Scene check
    public static string currentScene;
    public static string previousScene;

    public Transform currentSpawnPoint;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    void OnEnable()
    {
        GameStateManager.Instance.OnStateChange += HandleOnStateChange;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnStateChange -= HandleOnStateChange;
    }

    void Start()
    {
        // temp set to Play
        GameStateManager.Instance.SetGameState(GameState.Play);
    }

    void Update()
    {
        // test reset to title
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartToTitle();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RestartCurrentLevel();
        }
    }
    
    void HandleOnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartMenu:
                break;
            case GameState.Play:
                break;
            case GameState.Pause:
                break;
            case GameState.Fail:
                if (!enableGodMode) StartCoroutine(FailLoop());
                break;
            case GameState.Win:
                break;
        }
    }

    void RestartToTitle() {
        SceneManager.LoadScene("Opening");
    }

    IEnumerator FailLoop() {
        // play animation via coroutine etc etc
        yield return new WaitForSeconds(1f);
        RestartCurrentLevel();

    }

    void RestartCurrentLevel() {
        // reset enemy positions, player stats
        // pickups should not reset since we're not resetting score

        //player
        GameObject player = GameObject.Find("Player");
        PlayerStats pStats = player.GetComponent<PlayerStats>();
        pStats.ResetPlayerStats();
        player.transform.position = currentSpawnPoint.position;

        //music
        GameObject[] Triggers = GameObject.FindGameObjectsWithTag("Trigger");
        foreach (GameObject trigger in Triggers) {
            LevelBGMchanger o = trigger.GetComponent<LevelBGMchanger>();
            if (o != null) o.Reset();
        }

    }
}
