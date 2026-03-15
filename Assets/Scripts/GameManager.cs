using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float respawnDelay = 8f;
    
    [Header("Test Flags")]
    public bool enableGodMode = false;
    public bool startAtLevel1 = true;
    public bool startAtLevel2 = false;
    public bool startAtLevel3 = false;

    [HideInInspector] public string startLevel;

    [Header("Level Backgrounds Color")]
    public Color level1Color = new Color32(49, 77, 120, 255);
    public Color level2Color = new Color32(41, 64, 98, 255);
    public Color level3Color = new Color32(60, 52, 94, 255);

    [Header("Lighting")] 
    [HideInInspector] public Light2D globalLight;
    [HideInInspector] private Volume globalVolume;
    [HideInInspector] public Bloom globalBloom;
    public float level1LightIntensity = 0.4f;
    public float level2LightIntensity = 0.3f;
    public float level3LightIntensity = 0.2f;
    public float level1BloomIntensity = 10f;
    public float level2BloomIntensity = 12.5f;
    public float level3BloomIntensity = 15f;
    
    // Scene check
    public static string currentScene;
    public static string previousScene;

    public Transform currentSpawnPoint;

    // UI check
    private bool isGameOverObjRdy = false;
    private bool isPauseObjRdy = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (startLevel == null) startLevel = "Level1";
        else
        {
            if (startAtLevel1) startLevel = "Level1";
            else if (startAtLevel2) startLevel = "Level2";
            else if (startAtLevel3) startLevel = "Level3";
        }
    }
    
    void OnEnable()
    {
        GameStateManager.Instance.OnStateChange += HandleOnStateChange;
        GameOverMenuManager.OnGameOverMenuReady += HandleGameOverMenuReady;
        PauseBtnManager.OnPauseScreenReady += HandlePauseScreenReady;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnStateChange -= HandleOnStateChange;
        GameOverMenuManager.OnGameOverMenuReady -= HandleGameOverMenuReady;
        PauseBtnManager.OnPauseScreenReady -= HandlePauseScreenReady;
    }

    void Start()
    {
        // temp set to Play (GameManager is loaded in level)
        GameStateManager.Instance.SetGameState(GameState.Play);
        globalLight = FindAnyObjectByType<Light2D>();
        globalVolume = FindAnyObjectByType<Volume>();
        globalVolume.profile.TryGet<Bloom>(out globalBloom);
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
                ReturnToStartMenu();
                break;
            case GameState.Play:
                OnPlay();
                break;
            case GameState.Pause:
                OnPause();
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
        SceneManager.LoadScene("Opening");
        AudioManager.PlayMusic(SoundID.TitleScreen);
    }

    IEnumerator FailLoop() {
        // play animation via coroutine etc etc
        yield return new WaitForSeconds(1f);
        
        UIManager.instance.GameOverScreen.Show();
        float timer = 0f;
        while (timer < respawnDelay && !Input.GetMouseButtonDown(0))
        {
            timer += Time.deltaTime;
            yield return null;
        }
        RestartCurrentLevel();
    }

    public void RestartCurrentLevel() {
        // reset enemy positions, player stats
        // pickups should not reset since we're not resetting score

        //player
        GameObject player = GameObject.Find("Player");
        PlayerStats pStats = player.GetComponent<PlayerStats>();
        pStats.ResetPlayerStats();
        if (currentSpawnPoint == null) currentSpawnPoint = GameObject.Find("SpawnPoint").transform;
        player.transform.position = currentSpawnPoint.position;

        //music
        GameObject[] Triggers = GameObject.FindGameObjectsWithTag("Trigger");
        foreach (GameObject trigger in Triggers) {
            LevelBGMchanger o = trigger.GetComponent<LevelBGMchanger>();
            if (o != null) o.Reset();
        }

        // reset only bubble not treasure
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (GameObject pickup in pickups) {
            if (pickup.name.ToLower().Contains("bubble")) {
                pickup.GetComponent<BubbleScript>().Reset();
            }
        }

    }

    void HandleGameOverMenuReady(GameOverMenuManager menu)
    {
        isGameOverObjRdy = true;
        Debug.Log(UIManager.instance.GameOverScreen);
        if (UIManager.instance.GameOverScreen.gameObject.activeSelf)
        {
            UIManager.instance.GameOverScreen.gameObject.SetActive(false);
        }
    }

    void HandlePauseScreenReady(GameObject pause)
    {
        isPauseObjRdy = true;
        Debug.Log(UIManager.instance.PauseScreen);
        if (UIManager.instance.PauseScreen.activeSelf)
        {
            UIManager.instance.PauseScreen.SetActive(false);
        }
        Time.timeScale = 1f;
    }
}