using UnityEngine;

public class BubbleScript : MonoBehaviour, IPickup
{
    [SerializeField]
    [Range(1f, 10f)]
    private float oxygenGiven = 5f;

    public void PlayerContact(PlayerStats player)
    {
        player.AddOxygen(oxygenGiven);
    }

    public void SetOxygenValue(float value)
    {
        oxygenGiven = Mathf.Min(value, 10);
    }
}
