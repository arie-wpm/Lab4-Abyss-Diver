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

    [SerializeField]
    private float currentOxygenLevel;
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
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        oxygenBarGraphic.color = Color.turquoise;
    }

    // Update is called once per frame
    void Update()
    {
        HandleOxygenDrain();
    }

    private float GetCurrentOxygenPercent()
    {
        return currentOxygenLevel / maxOxygenLevel;
    }

    public void AddOxygen(float value)
    {
        currentOxygenLevel = Mathf.Min(maxOxygenLevel, currentOxygenLevel += value);
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
    }

    public void AddScore(float value)
    {
        score += value;
        DebugTool.Log($"Added: {value} score. Current Score: {score}");
    }

    private void HandleOxygenDrain()
    {
        currentOxygenLevel = Mathf.MoveTowards(
            currentOxygenLevel,
            0,
            oxygenDrainRate * Time.deltaTime
        );
        oxygenBarGraphic.fillAmount = GetCurrentOxygenPercent();
        if (GetCurrentOxygenPercent() >= 0.2f)
        {
            oxygenBarGraphic.color = Color.turquoise;
        }
        if (GetCurrentOxygenPercent() < 0.2f)
        {
            oxygenBarGraphic.color = Color.red;
        }
    }
}
