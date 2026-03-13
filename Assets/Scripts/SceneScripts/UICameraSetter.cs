using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICameraSetter : MonoBehaviour
{
    public Canvas canvas;
    public Image healthFill, oxygenFill;
    public TMP_Text score;

    void Start() {
        if (canvas == null) canvas = GetComponent<Canvas>();

        Camera mainCam = Camera.main;

        if (mainCam != null) canvas.worldCamera = mainCam;
        else Debug.LogWarning("No Main Camera found in the scene!");
    }
}