using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Offsets")]
    [SerializeField] private float horizontalOffset = 2f;
    [SerializeField] private float verticalOffset = 2f;

    [Header("Smoothing")]
    [SerializeField] private float followSpeed = 6f;
    [SerializeField] private float offsetSmoothSpeed = 4f;

    [Header("Direction Confirm Time")]
    [SerializeField] private float confirmTime = 0.5f;

    private float currentOffsetX;
    private float currentOffsetY;

    private float targetOffsetX;
    private float targetOffsetY;

    private float xDirectionTimer;
    private float yDirectionTimer;

    private int lastXDirection;
    private int lastYDirection;

    void LateUpdate() {
        if (player == null || playerRb == null) return;

        UpdateDirectionTimers();

        currentOffsetX = Mathf.Lerp(currentOffsetX, targetOffsetX, offsetSmoothSpeed * Time.deltaTime);
        currentOffsetY = Mathf.Lerp(currentOffsetY, targetOffsetY, offsetSmoothSpeed * Time.deltaTime);

        Vector3 targetPos = new Vector3(
            player.position.x + currentOffsetX,
            player.position.y + currentOffsetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    void UpdateDirectionTimers() {
        float vx = playerRb.linearVelocity.x;
        float vy = playerRb.linearVelocity.y;

        int xDir = Mathf.Abs(vx) > 0.1f ? (vx > 0 ? 1 : -1) : 0;
        int yDir = Mathf.Abs(vy) > 0.1f ? (vy > 0 ? 1 : -1) : 0;

        if (xDir != 0) {
            if (xDir == lastXDirection) xDirectionTimer += Time.deltaTime;
            else {
                xDirectionTimer = 0f;
                lastXDirection = xDir;
            }

            if (xDirectionTimer >= confirmTime) targetOffsetX = xDir * horizontalOffset;
        }

        if (yDir != 0) {
            if (yDir == lastYDirection) yDirectionTimer += Time.deltaTime;
            else {
                yDirectionTimer = 0f;
                lastYDirection = yDir;
            }

            if (yDirectionTimer >= confirmTime) targetOffsetY = yDir * verticalOffset;
        }
    }
}