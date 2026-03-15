using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool enableDebug = false;
    private PlayerStats pStats;
    private PlayerController pController;
    private Rigidbody2D pRigidBody;

    void Awake()
    {
        DebugTool.EnableLogging(nameof(PlayerCollisionHandler), enableDebug);
    }

    void Start()
    {
        pStats = PlayerStats.GlobalPlayerStats;
        pController = GetComponent<PlayerController>();
        pRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pickup":
                AudioManager.Play(SoundID.Pickup);
                DebugTool.Log("Touched Bubble");
                collision.GetComponent<IPickup>().PlayerContact(pStats);
                break;
            case "Current":
                //Activates the Water Current Movement.
                collision.GetComponent<CurrentScript>().StartMoveObject(pRigidBody);
                pController.InCurrent = true;
                break;
            case "Enemy":
                //Ememy collisions
                PlaySoundBasedOnEnemyType(collision);
                Debug.Log(
                    "Enemy Collided: "
                        + collision.GetComponent<EnemyDamageDealer>().damageSourceType
                );
                Vector2 heading = transform.position - collision.transform.position;
                pStats.TakeDamage(
                    collision.GetComponent<EnemyDamageDealer>().GetDamageFromType(),
                    heading.normalized
                );
                break;
            default:
                DebugTool.Log($"No case set for tag: {collision.tag}.");
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Pickup":
                break;
            case "Current":
                //Stops the Water Current Movement.
                collision.GetComponent<CurrentScript>().StopMoveObject(pRigidBody);
                pController.InCurrent = false;
                break;
            default:
                DebugTool.Log($"No case set for tag: {collision.tag}.");
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Hazard":
                PlaySoundBasedOnEnemyType(collision.collider);
                Vector2 heading = transform.position - collision.transform.position;
                pStats.TakeDamage(1, heading.normalized);
                break;
        }
    }

    void OnCollisionExit2D(Collision2D collision) { }

    void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Current":
                //Stops player moving while in the current; in stay to avoid player clipping the current and losing control.
                pController.InCurrent = true;
                break;
            default:
                DebugTool.Log($"No stay case set for tag: {collision.tag}.");
                break;
        }
    }

    void PlaySoundBasedOnEnemyType(Collider2D collision)
    {
        if (CameraHelper.IsInCameraBounds(collision.transform))
        {
            if (collision.name.ToLower().Contains("lantern"))
                AudioManager.Play(SoundID.LanternHit);
            if (collision.name.ToLower().Contains("seaurchin"))
                AudioManager.Play(SoundID.SpikeHit);
            if (collision.name.ToLower().Contains("spike"))
                AudioManager.Play(SoundID.SpikeHit);
            if (collision.name.ToLower().Contains("jelly"))
                AudioManager.Play(SoundID.JellyHit);
        }
    }
}
