using System.Collections;
using UnityEngine;

public class BubbleScript : MonoBehaviour, IPickup {
    [SerializeField, Range(1f, 10f)]
    private float oxygenGiven = 5f;

    private Animator anim;
    private CircleCollider2D col;
    private SpriteRenderer[] sprites;
    private ParticleSystem[] particles;
    private Coroutine disableCoroutine;

    void Awake() {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        particles = GetComponentsInChildren<ParticleSystem>();

        if (anim != null) anim.Play("BubbleMove", 0, Random.value);
    }

    public void PlayerContact(PlayerStats player) {
        player.AddOxygen(oxygenGiven);

        if (anim != null) anim.SetBool("isEmpty", true);

        col.enabled = false;

        if (disableCoroutine != null) StopCoroutine(disableCoroutine);
        disableCoroutine = StartCoroutine(DisableAfterAnimation());
    }

    IEnumerator DisableAfterAnimation() {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        foreach (var sprite in sprites) sprite.enabled = false;
        foreach (var ps in particles) ps.Stop();
        disableCoroutine = null;
    }

    public void SetOxygenValue(float value) {
        oxygenGiven = Mathf.Min(value, 10);
    }

    public void Reset() {
        if (disableCoroutine != null) {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }

        if (anim != null) {
            anim.Rebind();
            anim.Play("BubbleMove", 0, Random.value);
        }

        col.enabled = true;
        foreach (var sprite in sprites) sprite.enabled = true;
        foreach (var ps in particles) ps.Play();
    }
}