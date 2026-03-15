using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHeightLighting : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    [SerializeField]private Transform player;

    [SerializeField] private Volume globalVolume;
    [SerializeField] private Light2D sceneLight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        globalVolume = FindAnyObjectByType<Volume>();
        sceneLight = FindAnyObjectByType<Light2D>();
    }

    void Update()
    {
        float t = Mathf.InverseLerp(bottomPoint.position.y, topPoint.position.y, player.position.y);

        globalVolume.weight = t;
        sceneLight.intensity = Mathf.Lerp(0.5f, 1.5f, t);
    }
}