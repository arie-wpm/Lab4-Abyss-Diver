using UnityEngine;

public class ObjectColor : MonoBehaviour
{
    [ColorUsage(showAlpha: true, hdr: true)]
    [SerializeField] private Color color = Color.white;

    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        SetColor(color);
    }

    public void SetColor(Color newColor)
    {
        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor("_Color", newColor);
        _renderer.SetPropertyBlock(_mpb);
    }
}
