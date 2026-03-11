using UnityEngine;

public class Grass : MonoBehaviour
{
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            anim.SetBool("isTouched", true);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            anim.SetBool("isTouched", false);
        }
    }
}