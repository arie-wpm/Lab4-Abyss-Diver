using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Test Flags")]
    public bool enableGodMode = false;

    [Header("Level Backgrounds Color")]
    public Color level1Color = new Color32(49, 77, 120, 255);
    public Color level2Color = new Color32(41, 64, 98, 255);
    public Color level3Color = new Color32(60, 52, 94, 255);

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

    void ReturnToStartMenu()
    {
        //disable player controls
        RestartToTitle();
    }

    void OnPlay()
    {        
        if (isPauseObjRdy && UIManager.instance.PauseScreen.activeSelf)
        {
            UIManager.instance.PauseScreen.SetActive(false);
        }
        Time.timeScale = 1f;
    }
    
    void OnPause()
    {
        Time.timeScale = 0f;
        UIManager.instance.PauseScreen.SetActive(true);
    }

    public void RestartToTitle() {
        isGameOverObjRdy = false;
        isPauseObjRdy = false;
        if (Camera.main.backgroundColor != level1Color) Camera.main.backgroundColor = level1Color;
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
