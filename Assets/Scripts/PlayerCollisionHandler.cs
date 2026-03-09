using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerStats pStats;

    void Start()
    {
        pStats = GetComponent<PlayerStats>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pickup":
                Debug.Log("Touched Bubble");
                collision.GetComponent<IPickup>().PlayerContact(pStats);
                break;
            default:
                Debug.Log($"No case set for tag: {collision.tag}.");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision) { }

    void OnCollisionEnter2D(Collision2D collision) { }

    void OnCollisionExit2D(Collision2D collision) { }
}
