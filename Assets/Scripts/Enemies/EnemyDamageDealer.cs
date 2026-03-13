using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public enum DamageSourceType
    {
        Jellyfish,
        LanternFish,
        SeaUrchin,
        //Volcano,
        DeepSeaFissure,
        Spike
    }

    public DamageSourceType damageSourceType;

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        int damage = GetDamageFromType(damageSourceType);

        PlayerStats.GlobalPlayerStats.LoseHealth(damage);
    } 
    */

    public int GetDamageFromType()
    {
        switch (damageSourceType)
        {
            case DamageSourceType.Jellyfish:
                return 1;

            case DamageSourceType.LanternFish:
                return 1;

            case DamageSourceType.SeaUrchin:
                return 1;

            // case DamageSourceType.Volcano:
            //     return 1;

            case DamageSourceType.Spike:
                return 1;

            case DamageSourceType.DeepSeaFissure:
                return 999;
            default:
                return 1;
        }
    }
}