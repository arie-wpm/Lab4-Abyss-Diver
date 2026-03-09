using UnityEngine;

public class TreasureScript : MonoBehaviour, IPickup
{
    [SerializeField]
    private float scoreGiven = 500f;

    public void PlayerContact(PlayerStats player)
    {
        player.AddScore(scoreGiven);
    }

    public void SetOxygenValue(float value)
    {
        scoreGiven = value;
    }
}
