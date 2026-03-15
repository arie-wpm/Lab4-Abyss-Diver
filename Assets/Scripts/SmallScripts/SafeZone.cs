using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool enableDebug;
    private PlayerStats pStats;

    [SerializeField]
    private GameObject bubblePrefab;

    private BubbleScript myBubble;

    [SerializeField]
    private float respawnTime;
    private float currentRespawnTime;

    private bool hasBubble;

    void Start()
    {
        DebugTool.EnableLogging(nameof(SafeZone), enableDebug);
        currentRespawnTime = respawnTime;
        myBubble = Instantiate(bubblePrefab, transform).GetComponent<BubbleScript>();
    }

    void Update()
    {
        if (!myBubble.GetComponent<BubbleScript>().isActive)
        {
            currentRespawnTime -= Time.deltaTime;
            DebugTool.Log("CountDown");
        }
        if (currentRespawnTime <= 0f)
        {
            DebugTool.Log("SpawnBubble");
            myBubble.Reset();
            currentRespawnTime = respawnTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (pStats == null)
                pStats = other.gameObject.GetComponent<PlayerStats>();
            if (!pStats.isSafeZone)
                pStats.isSafeZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            pStats = other.gameObject.GetComponent<PlayerStats>();
            pStats.isSafeZone = false;
        }
    }
}
