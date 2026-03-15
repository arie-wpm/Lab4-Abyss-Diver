using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float horizontalDeadzone = 0.1f;

    [SerializeField]
    private float verticalHoldThreshold = 0.25f;

    [SerializeField]
    private float noInputThreshold = 0.5f;

    [SerializeField]
    private Transform particlePos;

    [SerializeField]
    private ParticleSystem bubbleFXPrefab;
    private ParticleSystem bubbleFX;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D collider;
    private InputAction moveAction;
    private Vector2 lastMoveValue;
    private float verticalHoldTimer;
    private float noInputTimer;
    private bool wasDown = false;
    private Coroutine playerBubbleRoutine;
    private PlayerStats pStats;

    void Awake()
    {
        pStats = GetComponent<PlayerStats>();
        pStats.OnPlayerDeath += HandlePlayerDeath;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        bubbleFX = Instantiate(
            bubbleFXPrefab,
            particlePos.position,
            Quaternion.identity,
            particlePos
        );
        playerBubbleRoutine = StartCoroutine(PlayerBubbleRoutine());
    }

    void OnDestroy()
    {
        pStats.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        Vector2 velocity = rb.linearVelocity;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed())
        {
            if (moveInput.x != 0)
            {
                lastMoveValue.x = moveInput.x;
                verticalHoldTimer = 0f;
            }
            if (moveInput.x == 0 && moveInput.y != 0)
                lastMoveValue.y = moveInput.y;
            noInputTimer = 0f;
        }

        bool isRight = false;
        bool isLeft = false;
        bool isUp = false;
        bool isDown = false;

        if (Mathf.Abs(moveInput.y) > 0)
            verticalHoldTimer += Time.deltaTime;
        else
            verticalHoldTimer = 0f;

        bool verticalActive = verticalHoldTimer >= verticalHoldThreshold;

        noInputTimer += Time.deltaTime;
        bool noInputActive = noInputTimer >= noInputThreshold;

        if (verticalActive || noInputActive)
        {
            isUp = moveInput.y > 0;
            isDown = moveInput.y < 0;
            lastMoveValue = moveInput;
        }
        else
        {
            isRight = lastMoveValue.x > 0;
            isLeft = lastMoveValue.x < 0;

            if (velocity.x > horizontalDeadzone && lastMoveValue.x > 0)
            {
                isRight = true;
                isLeft = false;
            }

            if (velocity.x < -horizontalDeadzone && lastMoveValue.x < 0)
            {
                isLeft = true;
                isRight = false;
            }
        }

        if (velocity == Vector2.zero)
            lastMoveValue = Vector2.zero;
        SetAnimParams(isRight, isLeft, isUp, isDown);

        if (isRight || isLeft)
        {
            collider.direction = CapsuleDirection2D.Horizontal;
        }
        else
        {
            collider.direction = CapsuleDirection2D.Vertical;
        }

        if (isDown && !wasDown)
        {
            bubbleFX.Play();
        }
        else if (!isDown && wasDown)
        {
            bubbleFX.Stop();
        }

        wasDown = isDown;

        //sfx hook
        if (isRight || isLeft || isUp || isDown)
        {
            if (!AudioManager.IsSwimPlaying())
                AudioManager.PlaySwim(SoundID.PlayerMove);
            else
                AudioManager.ResumeSwim();
        }
        else
        {
            if (AudioManager.IsSwimPlaying())
                AudioManager.StopSwim(SoundID.PlayerMove);
        }
    }

    void SetAnimParams(bool isRight, bool isLeft, bool isUp, bool isDown)
    {
        animator.SetBool("isRight", isRight);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isUp", isUp);
        animator.SetBool("isDown", isDown);
    }

    void SetAnimAllFalse() {
        animator.SetBool("isRight", false);
        animator.SetBool("isLeft", false);
        animator.SetBool("isUp", false);
        animator.SetBool("isDown", false);
    }

    IEnumerator PlayerBubbleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            float waitTime = Random.Range(2f, 4f);
            yield return new WaitForSeconds(waitTime);
            bubbleFX.Play();
            float playDuration = Random.Range(1f, 1.5f);
            yield return new WaitForSeconds(playDuration);
            bubbleFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void StopRandomBubbles()
    {
        if (playerBubbleRoutine != null)
        {
            StopCoroutine(playerBubbleRoutine);
            playerBubbleRoutine = null;
        }
        if (bubbleFX.isPlaying)
            bubbleFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void StartRandomBubbles()
    {
        if (playerBubbleRoutine == null)
            playerBubbleRoutine = StartCoroutine(PlayerBubbleRoutine());
    }

    void HandlePlayerDeath() => RunDeathAnim();

    void RunDeathAnim()
    {
        rb.simulated = false;
        rb.linearVelocity = Vector2.zero;
        SetAnimAllFalse();
        animator.SetBool("isDead", true);
    }

    public IEnumerator RunRespawnAnim()
    {
        SetAnimAllFalse();
        animator.Rebind();
        rb.simulated = false;
        animator.SetBool("isRespawn", true);

        yield return null;

        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);

        animator.SetBool("isRespawn", false);
        rb.simulated = true;
    }

}
