using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        GameManager.instance.currentSpawnPoint = spawnPoint;
    }
}
