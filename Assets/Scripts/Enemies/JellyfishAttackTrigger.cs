using System.Collections;
using UnityEngine;

public class JellyfishAttackTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool canZap = false;
    [SerializeField] private float timeBetweenZaps = 3f;
    private Coroutine jellyZapRoutine;
    private bool isZapping = false;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }

        if (canZap) jellyZapRoutine = StartCoroutine(JellyZap());
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

    IEnumerator JellyZap()
    {
        while (canZap)
        {
            yield return new WaitForSeconds(timeBetweenZaps);
            animator.SetBool("isCharging", true);
            AudioManager.Play(SoundID.JellyCharge);
            yield return new WaitForSeconds(2.5f);
            animator.SetBool("isCharging", false);
            animator.SetBool("isZapping", true);
            isZapping = true;
            AudioManager.Play(SoundID.JellyZap);
            yield return new WaitForSeconds(1.1f);
            animator.SetBool("isZapping", false);
            isZapping = false;
            animator.SetBool("IsAttacking", false);
        }
    }

    public void PlayChargeSFX()
    {
        // sorry leaving this here cbf removing marker from aniamtor lol
    }

    public void PlayZapSFX()
    {
        // sorry leaving this here cbf removing marker from aniamtor lol
    }
}