using UnityEngine;

public static class CameraHelper {

    public static bool IsInCameraBounds(Transform target, float padding = 0.15f) {
        if (target == null || Camera.main == null) return false;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(target.position);

        return viewportPos.z > 0 &&
            viewportPos.x > -padding && viewportPos.x < 1 + padding &&
            viewportPos.y > -padding && viewportPos.y < 1 + padding;
    }

    public static bool IsInCameraBounds(GameObject obj, float padding = 0.15f) {
        if (obj == null) return false;
        return IsInCameraBounds(obj.transform, padding);
    }
}