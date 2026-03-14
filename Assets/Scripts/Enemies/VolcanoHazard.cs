using System.Collections;
using UnityEngine;

public class VolcanoHazard : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D damageHitbox;

    public float idleTime = 2.5f;
    public float warningTime = 0.8f;
    public float startEruptingTime = 0.5f;
    public float eruptingTime = 1.0f;
    public float endEruptingTime = 0.5f;

    [Header("Lava Collider")]
    public float minColliderHeight = 0.1f;
    public float maxColliderHeight = 2.0f;

    private float hitboxWidth;
    private Vector2 originalOffset;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (damageHitbox != null)
        {
            hitboxWidth = damageHitbox.size.x;
            originalOffset = damageHitbox.offset;

            SetColliderHeight(minColliderHeight);
            damageHitbox.enabled = false;
        }

        StartCoroutine(VolcanoCycle());
    }

    private IEnumerator VolcanoCycle()
    {
        while (true)
        {
            // idle
            animator.SetInteger("volcanoState", 0);
            SetDamageActive(false);
            SetColliderHeight(minColliderHeight);
            yield return new WaitForSeconds(idleTime);

            // warning
            animator.SetInteger("volcanoState", 1);
            SetDamageActive(false);
            SetColliderHeight(minColliderHeight);
            yield return new WaitForSeconds(warningTime);

            // lava rising
            animator.SetInteger("volcanoState", 2);
            SetDamageActive(true);
            yield return StartCoroutine(ResizeCollider(minColliderHeight, maxColliderHeight, startEruptingTime));

            // fully erupting
            animator.SetInteger("volcanoState", 3);
            SetDamageActive(true);
            SetColliderHeight(maxColliderHeight);
            yield return new WaitForSeconds(eruptingTime);

            // lava shrinking
            animator.SetInteger("volcanoState", 4);
            SetDamageActive(true);
            yield return StartCoroutine(ResizeCollider(maxColliderHeight, minColliderHeight, endEruptingTime));

            SetDamageActive(false);
        }
    }

    private IEnumerator ResizeCollider(float startHeight, float targetHeight, float duration)
    {
        if (damageHitbox == null)
        {
            yield break;
        }

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float newHeight = Mathf.Lerp(startHeight, targetHeight, t);
            SetColliderHeight(newHeight);

            time += Time.deltaTime;
            yield return null;
        }

        SetColliderHeight(targetHeight);
    }

    private void SetColliderHeight(float height)
    {
        if (damageHitbox == null)
        {
            return;
        }

        damageHitbox.size = new Vector2(hitboxWidth, height);
        damageHitbox.offset = new Vector2(originalOffset.x, originalOffset.y + height / 2f);
    }

    private void SetDamageActive(bool isActive)
    {
        if (damageHitbox != null)
        {
            damageHitbox.enabled = isActive;
        }
    }

    public void PlaySFX()
    {
        if (CameraHelper.IsInCameraBounds(transform, 2f)) AudioManager.Play(SoundID.Volcano);
    }
}