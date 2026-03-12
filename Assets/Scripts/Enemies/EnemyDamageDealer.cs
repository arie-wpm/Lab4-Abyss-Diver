using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public enum DamageSourceType
    {
        Jellyfish,
        LanternFish,
        ElectricEel,
        SeaUrchin,
        Volcano,
        DeepSeaFissure,
        Spike
    }

    public DamageSourceType damageSourceType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        int damage = GetDamageFromType(damageSourceType);

        // player health reduction
        // iframes
        // temporary collision ignoring?

        PlayerStats.GlobalPlayerStats.TakeDamage(damage);
    }

    private int GetDamageFromType(DamageSourceType type)
    {
        switch (type)
        {
            case DamageSourceType.Jellyfish:
                return 1;

            case DamageSourceType.LanternFish:
                return 1;

            case DamageSourceType.ElectricEel:
                return 2; // TODO: eel calm state - safe, eel electrified state - lose heart

            case DamageSourceType.SeaUrchin:
                return 1;

            case DamageSourceType.Volcano:
                return 1;

            case DamageSourceType.Spike:
                return 1;

            case DamageSourceType.DeepSeaFissure:
                return 999; // TODO: Later change so the player gets sucked in and dies instantly when hitting a fissure.
            default:
                return 1;
        }
    }
}