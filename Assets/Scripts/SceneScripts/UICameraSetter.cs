using UnityEngine;
using UnityEngine.UI;

public class UICameraSetter : MonoBehaviour
{
    public Canvas canvas;
    public Image healthFill, oxygenFill;

    void Start() {
        if (canvas == null) canvas = GetComponent<Canvas>();

        Camera mainCam = Camera.main;

        if (mainCam != null) canvas.worldCamera = mainCam;
        else Debug.LogWarning("No Main Camera found in the scene!");
    }
}