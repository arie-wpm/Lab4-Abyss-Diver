using System.Collections;
using UnityEngine;

public class BubbleScript : MonoBehaviour, IPickup
{
    [SerializeField]
    [Range(1f, 10f)]
    private float oxygenGiven = 5f;
    private Animator anim;
    private CircleCollider2D col;

    void Awake() {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        if (anim == null) return;
        anim.Play("BubbleMove", 0, Random.value);
    }

    public void PlayerContact(PlayerStats player)
    {
        player.AddOxygen(oxygenGiven);
        if (anim == null) return;
        anim.SetBool("isEmpty", true);
        col.enabled = false;
        StartCoroutine(DestoryAfterAnimation());
    }

    public void SetOxygenValue(float value)
    {
        oxygenGiven = Mathf.Min(value, 10);
    }

    IEnumerator DestoryAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
