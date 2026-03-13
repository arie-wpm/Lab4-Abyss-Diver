using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform pTrans;

    [Header("Camera Settings")]
    [SerializeField] private float followSpeed = 5f;

    [Header("Deadzone / Bounds")]
    [SerializeField] private Vector2 deadzoneSize = new Vector2(2f, 2f);

    [Header("World Bounds")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private Rigidbody2D pRb;

    void Start() {
        if (pTrans != null)
            pRb = pTrans.GetComponent<Rigidbody2D>();
    }

    void LateUpdate() {
        if (pTrans == null) return;
        FollowPlayer();
    }

    void FollowPlayer() {
        Vector3 camPos = transform.position;

        float left = camPos.x - deadzoneSize.x / 2;
        float right = camPos.x + deadzoneSize.x / 2;
        float bottom = camPos.y - deadzoneSize.y / 2;
        float top = camPos.y + deadzoneSize.y / 2;

        if (pRb.position.x > right)
            camPos.x += pRb.position.x - right;
        else if (pRb.position.x < left)
            camPos.x += pRb.position.x - left;

        if (pRb.position.y > top)
            camPos.y += pRb.position.y - top;
        else if (pRb.position.y < bottom)
            camPos.y += pRb.position.y - bottom;

        camPos.x = Mathf.Clamp(camPos.x, minBounds.x, maxBounds.x);
        camPos.y = Mathf.Clamp(camPos.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, camPos, followSpeed * Time.deltaTime);
    }

    void OnDrawGizmos() {
        if (pTrans == null) return;

        // Draw world bounds
        Gizmos.color = Color.yellow;
        Vector3 worldCenter = new Vector3((minBounds.x + maxBounds.x) / 2, (minBounds.y + maxBounds.y) / 2, 0);
        Vector3 worldSize = new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y, 0);
        Gizmos.DrawWireCube(worldCenter, worldSize);

        // Draw camera deadzone
        Gizmos.color = Color.cyan;
        Vector3 deadzoneCenter = transform.position;
        Vector3 deadzoneRect = new Vector3(deadzoneSize.x, deadzoneSize.y, 0);
        Gizmos.DrawWireCube(deadzoneCenter, deadzoneRect);
    }
}