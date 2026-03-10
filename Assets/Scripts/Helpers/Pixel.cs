using UnityEngine;

public class Pixel {

    public static float pixelsPerUnit = 32f;

    public static Vector3 Snap(Vector3 pos) {
        
        float snapValue = 1f / pixelsPerUnit;
        pos.x = Mathf.Round(pos.x / snapValue) * snapValue;
        pos.y = Mathf.Round(pos.y / snapValue) * snapValue;
        return pos;
    }

    public static Vector2 Snap(Vector2 pos) {
        float snapValue = 1f / pixelsPerUnit;
        pos.x = Mathf.Round(pos.x / snapValue) * snapValue;
        pos.y = Mathf.Round(pos.y / snapValue) * snapValue;
        return pos;
    }

}
