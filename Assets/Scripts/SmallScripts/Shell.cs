using UnityEngine;

public class Shell : MonoBehaviour
{
    private bool hasAnimPlayed = false;
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Update() {

        if (transform.childCount == 0 && !hasAnimPlayed)
        {
            hasAnimPlayed = true;
            anim.SetBool("isEmpty", true);
        }
    }

    // think about adding pearl as a reset function
}