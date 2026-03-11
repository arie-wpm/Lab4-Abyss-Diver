using UnityEngine;

public class DamageDealer : MonoBehaviour
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

        PlayerStats.GlobalPlayerStats.LoseHealth(damage);

        Debug.Log($"{damageSourceType} dealt {damage} damage");
    }

    private int GetDamageFromType(DamageSourceType type)
    {
        switch (type)
        {
            case DamageSourceType.Jellyfish:
            case DamageSourceType.LanternFish:
            case DamageSourceType.SeaUrchin:
            case DamageSourceType.Volcano:
                return 1;

            case DamageSourceType.ElectricEel:
                return 2;

            case DamageSourceType.DeepSeaFissure:
                return 999; //instant death?

            default:
                return 1;
        }
    }
}