using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool enableDebug = false;

    [Header("Player Reference")]
    [SerializeField] private Transform pTrans;

    [Header("Camera Settings")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [Header("Deadzone / Threshold")]
    [SerializeField] private Vector2 threshold;

    private Vector3 targetPos;
    private Rigidbody2D pRb;

    void Awake() {
        DebugTool.EnableLogging(nameof(CameraFollow), enableDebug);
    }

    void Start() {
        pRb = pTrans.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (pTrans == null) return;
        ClampPlayer();
    }

    void LateUpdate() {
        if (pTrans == null) return;
        ClampCamera();
    }

    void ClampCamera() {
        targetPos = transform.position;

        if (pRb.position.x > transform.position.x + threshold.x)
            targetPos.x = pRb.position.x - threshold.x;
        else if (pRb.position.x < transform.position.x - threshold.x)
            targetPos.x = pRb.position.x + threshold.x;

        if (pRb.position.y > transform.position.y + threshold.y)
            targetPos.y = pRb.position.y - threshold.y;
        else if (pRb.position.y < transform.position.y - threshold.y)
            targetPos.y = pRb.position.y + threshold.y;

        targetPos.z = transform.position.z;

        targetPos.x = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    void ClampPlayer() {
        Vector3 playerPos = pRb.position;
        playerPos.x = Mathf.Clamp(playerPos.x, minBounds.x, maxBounds.x);
        playerPos.y = Mathf.Clamp(playerPos.y, minBounds.y, maxBounds.y);
        pRb.position = playerPos;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2, (minBounds.y + maxBounds.y) / 2, 0);
        Vector3 size = new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y, 0);
        Gizmos.DrawWireCube(center, size);
    }
}