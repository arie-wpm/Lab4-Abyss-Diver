using UnityEngine;

public class JellyfishAttackTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        animator.SetBool("IsAttacking", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        animator.SetBool("IsAttacking", false);
    }
}