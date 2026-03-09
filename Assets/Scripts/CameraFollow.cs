using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool enableDebug = false;

    void Awake() {
        DebugTool.EnableLogging(nameof(CameraFollow), enableDebug);
    }

    void Update() {
        DebugTool.Log("Update called");
    }
}
