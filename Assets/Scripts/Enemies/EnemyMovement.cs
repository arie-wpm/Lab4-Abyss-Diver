using UnityEngine;

/*
FISH
    Moving > JellyFish, LanternFish /done/
    Stationary > ElectricEel 
HAZARDS
    Stationary > Sea Urchins
    Volcano
    Minecraft Magma Block thingy - Deep Sea crack?
    Ocean Currents?
*/

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType
    {
        Jellyfish,
        LanternFish,
        ElectricEel
    }

    public enum MovementType
    {
        StraightPatrol,
        WavyPatrol,
        Static
    }

    public EnemyType enemyType;

    [Header("Movement")]
    public MovementType movementType;
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float reachDistance = 0.05f;

    [Header("Wavy Movement")]
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 3f;

    private Transform currentTarget;
    private Vector3 basePosition;
    private float waveTime;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogWarning($"{gameObject.name}: Point A or Point B is missing.");
            enabled = false;
            return;
        }

        currentTarget = pointA;
        basePosition = transform.position;
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.StraightPatrol:
                PatrolStraight();
                break;

            case MovementType.WavyPatrol:
                PatrolWavy();
                break;

            case MovementType.Static:
                break;
        }
    }

    void PatrolStraight()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );

        CheckSwitchTarget(transform.position);
    }

    void PatrolWavy()
    {
        basePosition = Vector3.MoveTowards(
            basePosition,
            currentTarget.position,
            speed * Time.deltaTime
        );

        waveTime += Time.deltaTime;
        float yOffset = Mathf.Sin(waveTime * waveFrequency) * waveAmplitude;

        transform.position = basePosition + new Vector3(0f, yOffset, 0f);

        CheckSwitchTarget(basePosition);
    }

    void CheckSwitchTarget(Vector3 checkPosition)
    {
        if (Vector3.Distance(checkPosition, currentTarget.position) <= reachDistance)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}