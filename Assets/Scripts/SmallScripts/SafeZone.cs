using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private PlayerStats pStats;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (pStats == null) pStats = other.gameObject.GetComponent<PlayerStats>();
            if (!pStats.isSafeZone) pStats.isSafeZone = true;
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
