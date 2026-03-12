using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats GlobalPlayerStats { get; private set; }

    [Header("DebugTool.Log")]
    [SerializeField]
    private bool enableDebug = false;

    [Header("Oxygen")]
    [SerializeField]
    private float maxOxygenLevel;

    private float currentOxygenLevel;

    [HideInInspector]
    public float CurrentOxygenLevel
    {
        get => currentOxygenLevel;
    }

    [SerializeField]
    private float oxygenDrainRate;

    //Just for testing Purposes can be changed later
    [SerializeField]
    private Image oxygenBarGraphic;

    [SerializeField]
    public float baseOxygenToAdd;

    [Header("Health")]
    [SerializeField]
    private int maxHearts = 3;
    private bool isDrowning = false;

    private int currentHearts;

    [HideInInspector]
    public float CurrentHearts
    {
        get => currentHearts;
    }

    [SerializeField]
    private Image healthBarGraphic;

    //Before losing the first heart while drowning, waits slighly longer (additive to the regular timebetweenheartloss).
    [SerializeField]
    private float gracePeriod;

    //Time between losing a heart while drowning.
    [SerializeField]
    private float timeBetweenHeartLoss;

    [Header("Score")]
    [HideInInspector]
    public float score = 0f;

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
    }

    void Start()
    {
        currentOxygenLevel = maxOxygenLevel;
        currentHearts = maxHearts;
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        oxygenBarGraphic.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        HandleOxygenDrain();
        if (currentOxygenLevel <= 0 && !isDrowning)
        {
            isDrowning = true;
            StartCoroutine(HandleLoseHealth());
            HandleLoseHealth();
        }
    }

    private IEnumerator HandleLoseHealth()
    {
        yield return new WaitForSeconds(gracePeriod);
        while (currentHearts > 0)
        {
            yield return new WaitForSeconds(timeBetweenHeartLoss);
            LoseHealth(1);
            DebugTool.Log($"Lost Health. Current Hearts: {currentHearts}");
        }
        DebugTool.Log($"Died. Current Hearts: {currentHearts}");
    }

    public void LoseHealth(int healthToLose)
    {
        healthToLose = Mathf.Min(3, healthToLose);
        for (int i = 0; i < healthToLose; i++)
        {
            currentHearts--;
            healthBarGraphic.fillAmount -= .33f;
        }
    }

    public void TakeDamage(int amount)
    {
        // if (isInvincible) return;

        if (currentHearts <= 0)
            return;

        LoseHealth(amount);
        Debug.Log("Lost Health. Current Hearts: " + currentHearts);

        if (currentHearts <= 0)
        {
            currentHearts = 0;
            DebugTool.Log($"Died. Current Hearts: {currentHearts}");
            return;
        }

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
        DebugTool.Log($"Added: {value} score. Current Score: {score}");
    }

    private void HandleOxygenDrain()
    {
        currentOxygenLevel = Mathf.Max(
            0,
            Mathf.MoveTowards(currentOxygenLevel, 0, oxygenDrainRate * Time.deltaTime)
        );
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        if (GetCurrentOxygenPercent() >= 0.2f)
        {
            oxygenBarGraphic.color = Color.white;
        }
        if (GetCurrentOxygenPercent() < 0.2f)
        {
            oxygenBarGraphic.color = Color.red;
        }
    }
}
