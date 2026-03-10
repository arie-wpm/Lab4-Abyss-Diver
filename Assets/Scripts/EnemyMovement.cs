using UnityEngine;

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
        Patrol_AB,
        Vertical_AB,
        Static
    }

    public EnemyType enemyType;

    public MovementType movementType;

    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;
    public float reachDistance = 0.05f;

    private Transform currentTarget;

    void Start()
    {
        if (pointA != null)
            currentTarget = pointA;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        switch (movementType)
        {
            case MovementType.Patrol_AB:
                Patrol();
                break;

            case MovementType.Vertical_AB:
                VerticalMove();
                break;

            case MovementType.Static:
                break;
        }
    }

    void Patrol()
    {
        MoveTowardsTarget();

        if (ReachedTarget())
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            Flip();
        }
    }

    void VerticalMove()
    {
        MoveTowardsTarget();

        if (ReachedTarget())
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );
    }

    bool ReachedTarget()
    {
        return Vector3.Distance(transform.position, currentTarget.position) < reachDistance;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}