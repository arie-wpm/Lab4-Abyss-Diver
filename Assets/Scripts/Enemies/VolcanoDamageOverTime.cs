using UnityEngine;

public class VolcanoDamageOverTime : MonoBehaviour
{
    public float damageInterval = 1f;
    public int damageAmount = 1;

    private float timer = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        timer += Time.deltaTime;

        if (timer >= damageInterval)
        {
            PlayerStats.GlobalPlayerStats.TakeDamage(damageAmount);
            timer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        timer = 0f;
    }

    private void OnDisable()
    {
        timer = 0f;
    }
}