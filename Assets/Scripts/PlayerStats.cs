using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    public static PlayerStats GlobalPlayerStats { get; private set; }
    private PlayerController playerController;
    private DamageFlash damageFlash;
    
    [Header("DebugTool.Log")]
    [SerializeField] private bool enableDebug = false;

    [Header("Oxygen")]
    [SerializeField] private float maxOxygenLevel;
    private float currentOxygenLevel;

    [HideInInspector] public float CurrentOxygenLevel
    {
        get => currentOxygenLevel;
    }

    [SerializeField] private float oxygenDrainRate;

    //Just for testing Purposes can be changed later
    [SerializeField] private Image oxygenBarGraphic;
    [SerializeField] public float baseOxygenToAdd;

    [Header("Health")]
    [SerializeField] private int maxHearts = 3;
    private bool isDrowning = false;
    private int currentHearts;

    [HideInInspector] public float CurrentHearts
    {
        get => currentHearts;
    }

    private bool isInvincible;

    public bool IsInvincible
    {
        get => isInvincible;
        set => isInvincible = value;
    }

    [SerializeField] private Image healthBarGraphic;
    //Before losing the first heart while drowning, waits slighly longer (additive to the regular timebetweenheartloss).
    [SerializeField] private float gracePeriod;
    //Time between losing a heart while drowning.
    [SerializeField] private float timeBetweenHeartLoss;
    [HideInInspector] public float score = 0f;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreboard;
    private bool isUILinked = false;
    private bool hasOxyLowPlayed = false;

    public bool isSafeZone = false;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isUILinked = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "UI") {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj != null) {
                healthBarGraphic = canvasObj.GetComponentInChildren<UICameraSetter>().healthFill;
                oxygenBarGraphic = canvasObj.GetComponentInChildren<UICameraSetter>().oxygenFill;
                scoreboard = canvasObj.GetComponentInChildren<UICameraSetter>().score;
            }
            currentOxygenLevel = maxOxygenLevel;
            currentHearts = maxHearts;
            oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
            oxygenBarGraphic.color = Color.white;
            isUILinked = true;
        }
    }

    void Awake()
    {
        if (GlobalPlayerStats != null && GlobalPlayerStats != this)
        {
            Destroy(this);
        }
        else
        {
            GlobalPlayerStats = this;
        }
        DebugTool.EnableLogging(nameof(PlayerStats), enableDebug);

        playerController = GetComponent<PlayerController>();
        damageFlash = GetComponent<DamageFlash>();
        isInvincible = false;
    }

    void Start()
    {
        // currentOxygenLevel = maxOxygenLevel;
        // currentHearts = maxHearts;
        // oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        // oxygenBarGraphic.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUILinked) {
            if (!isSafeZone) HandleOxygenDrain();
            if (currentOxygenLevel <= 0 && !isDrowning)
            {
                isDrowning = true;
                StartCoroutine(HandleDrown());
            }
        }
    }

    private IEnumerator HandleDrown()
    {
        yield return new WaitForSeconds(gracePeriod);
        while (currentHearts > 0)
        {
            yield return new WaitForSeconds(timeBetweenHeartLoss);
            LoseHealth(1);
            DebugTool.Log($"Lost Health. Current Hearts: {currentHearts}");
        }
        DebugTool.Log($"Died. Current Hearts: {currentHearts}");
        GameStateManager.Instance.SetGameState(GameState.Fail);
    }

    public void LoseHealth(int healthToLose)
    {
        AudioManager.Play(SoundID.Hurt);
        if (GameManager.instance.enableGodMode) return;
        healthToLose = Mathf.Min(3, healthToLose);
        for (int i = 0; i < healthToLose; i++)
        {
            currentHearts--;
            healthBarGraphic.fillAmount -= .33f;
        }
    }

    public void TakeDamage(int amount, Vector2 dir)
    {
        if (isInvincible) return;

        if (currentHearts <= 0) {
            return;
        }

        LoseHealth(amount);
        damageFlash.CallDamageFlash();
        Debug.Log("Lost Health. Current Hearts: " + currentHearts);
        playerController.Knockback(dir);

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            playerController.CanMove = false;
            DebugTool.Log($"Died. Current Hearts: {currentHearts}");
            return;
        }
        
        
        // commenting out this will never run
        // if (currentHearts <= 0)
        // {
        //     currentHearts = 0;
        //     DebugTool.Log($"Died. Current Hearts: {currentHearts}");      
        //     return;
        // }

        // iframe?
    }

    public void AddHealth(int healthToAdd)
    {
        healthToAdd = Mathf.Min(3 - currentHearts, healthToAdd);
        for (int i = 0; i < healthToAdd; i++)
        {
            currentHearts++;
            healthBarGraphic.fillAmount += .33f;
        }
    }

    private float GetCurrentOxygenPercent()
    {
        return currentOxygenLevel / maxOxygenLevel;
    }

    public void AddOxygen(float value)
    {
        currentOxygenLevel = Mathf.Min(maxOxygenLevel, currentOxygenLevel += value);
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        isDrowning = false;
    }

    public void AddScore(float value)
    {
        score += value;
        scoreboard.SetText(score.ToString("00000"));
    }

    private void HandleOxygenDrain()
    {
        currentOxygenLevel = Mathf.Max(
            0,
            Mathf.MoveTowards(currentOxygenLevel, 0, oxygenDrainRate * Time.deltaTime)
        );
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        if (GetCurrentOxygenPercent() >= 0.3f)
        {
            oxygenBarGraphic.color = Color.white;
            hasOxyLowPlayed = false;
        }

        if (GetCurrentOxygenPercent() < 0.3f)
        {
            if (!hasOxyLowPlayed) {
                AudioManager.Play(SoundID.OxyWarning);
                hasOxyLowPlayed = true;
            }
            oxygenBarGraphic.color = Color.red;
        }
    }

    public void ResetPlayerStats() {
        isDrowning = false;
        hasOxyLowPlayed = false;
        currentOxygenLevel = maxOxygenLevel;
        currentHearts = maxHearts;
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        oxygenBarGraphic.color = Color.white;
        healthBarGraphic.fillAmount = 1f;
    }
}
