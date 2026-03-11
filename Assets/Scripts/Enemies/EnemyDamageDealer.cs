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
        DeepSeaFissure
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
                return 2;

            case DamageSourceType.SeaUrchin:
                return 1;

            case DamageSourceType.Volcano:
                return 1;

            case DamageSourceType.DeepSeaFissure:
                return 999;

            default:
                return 1;
        }
    }
}