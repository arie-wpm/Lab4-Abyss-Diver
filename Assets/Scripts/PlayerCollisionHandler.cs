using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool enableDebug = false;
    private PlayerStats pStats;

    void Awake() {
        DebugTool.EnableLogging(nameof(PlayerCollisionHandler), enableDebug);
    }

    void Start()
    {
        pStats = GetComponent<PlayerStats>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pickup":
                DebugTool.Log("Touched Bubble");
                collision.GetComponent<IPickup>().PlayerContact(pStats);
                break;
            default:
                DebugTool.Log($"No case set for tag: {collision.tag}.");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision) { }

    void OnCollisionEnter2D(Collision2D collision) { }

    void OnCollisionExit2D(Collision2D collision) { }
}
