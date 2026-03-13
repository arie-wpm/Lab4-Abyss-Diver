using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
                break;
            case GameState.Win:
                break;
        }
    }

    void RestartToTitle() {
        SceneManager.LoadScene("Opening");
    }

    void RestartCurrentLevel() {
        // reset enemy positions, player stats

        GameObject player = GameObject.Find("Player");
        PlayerStats pStats = player.GetComponent<PlayerStats>();
        pStats.ResetPlayerStats();
        player.transform.position = currentSpawnPoint.position;
    }
}
