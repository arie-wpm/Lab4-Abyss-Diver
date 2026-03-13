using UnityEngine;

public class FissurePullZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform fissureCenter;

    [Header("Pull Settings")]
    [SerializeField] private float pullForce = 1.8f;
    [SerializeField] private float maxExtraPullSpeed = 2.5f;

    private void Reset()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D playerRb = other.attachedRigidbody;
        if (playerRb == null) return;
        if (fissureCenter == null) return;

        Vector2 toFissure = (Vector2)(fissureCenter.position - other.transform.position);
        float distance = toFissure.magnitude;

        if (distance <= 0.01f) return;

        Vector2 direction = toFissure.normalized;

        // pull stronger when closer
        float scaledPull = pullForce / Mathf.Max(distance, 0.75f);

        playerRb.AddForce(direction * scaledPull, ForceMode2D.Force);
        Vector2 velocity = playerRb.linearVelocity;
        if (velocity.magnitude > maxExtraPullSpeed + 4f)
        {
            playerRb.linearVelocity = velocity.normalized * (maxExtraPullSpeed + 4f);
        }
    }
}