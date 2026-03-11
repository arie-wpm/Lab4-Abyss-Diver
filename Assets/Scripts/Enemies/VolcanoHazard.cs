using System.Collections;
using UnityEngine;

public class VolcanoHazard : MonoBehaviour
{
    public Animator animator;
    public Collider2D damageHitbox;

    public float idleTime = 2.5f;
    public float warningTime = 0.8f;
    public float startEruptingTime = 0.5f;
    public float eruptingTime = 1.0f;
    public float endEruptingTime = 0.5f;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (damageHitbox != null)
        {
            damageHitbox.enabled = false;
        }

        StartCoroutine(VolcanoCycle());
    }

    private IEnumerator VolcanoCycle()
    {
        while (true)
        {
            // Idle
            animator.SetInteger("volcanoState", 0);
            SetDamageActive(false);
            yield return new WaitForSeconds(idleTime);

            // warning
            animator.SetInteger("volcanoState", 1);
            SetDamageActive(false);
            yield return new WaitForSeconds(warningTime);

            // lava rising
            animator.SetInteger("volcanoState", 2);
            SetDamageActive(true);
            yield return new WaitForSeconds(startEruptingTime);

            // fully erupting
            animator.SetInteger("volcanoState", 3);
            SetDamageActive(true);
            yield return new WaitForSeconds(eruptingTime);

            // lava shrinking
            animator.SetInteger("volcanoState", 4);
            SetDamageActive(true);
            yield return new WaitForSeconds(endEruptingTime);
        }
    }

    private void SetDamageActive(bool isActive)
    {
        if (damageHitbox != null)
        {
            damageHitbox.enabled = isActive;
        }
    }
}