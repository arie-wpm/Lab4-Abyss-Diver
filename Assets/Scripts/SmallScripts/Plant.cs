using UnityEngine;

public class Plant : MonoBehaviour
{
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anim.Play("PlantMove", 0, Random.value);
    }
}
