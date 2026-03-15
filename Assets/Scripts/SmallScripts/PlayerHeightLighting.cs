using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerHeightLighting : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    [SerializeField]private Transform player;

    [SerializeField] private Volume globalVolume;
    [SerializeField] private Color bgColor;
    private Color defaultBgColor;

    private bool hasInit = false;

    void Start()
    {
        defaultBgColor = GameManager.instance.level1Color;
        StartCoroutine(Init());
    }

    void Update() {
        if (!hasInit) return;

        float t = Mathf.InverseLerp(bottomPoint.position.y, topPoint.position.y, player.position.y);
        float u;
        float s;
        if (t <= 0.5f) {
            u = Mathf.InverseLerp(0f, 0.5f, t);
            s = Mathf.Abs(1f - u);
        }
        else {
            u = 1f;
            s = 0f;
        }

        globalVolume.weight = s;
        Camera.main.backgroundColor = Color.Lerp(defaultBgColor, bgColor, t);
    }

    IEnumerator Init() {
        yield return null;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (globalVolume == null) globalVolume = FindAnyObjectByType<Volume>();
        hasInit = true;
    }
}