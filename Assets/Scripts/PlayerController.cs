using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private bool enableDebug = false;

    [Header("Movement")]
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float ascendAcceleration;

    [SerializeField]
    private float descendAcceleration;

    [SerializeField]
    private float horizontalAcceleration;

    [SerializeField]
    private float maxHorizontalVelocity;

    [SerializeField]
    private float maxAscendVelocity;

    [SerializeField]
    private float maxDescendVelocity;

    [SerializeField]
    private float maxGravityVelocity;

    [SerializeField]
    private float postDescendSlowDownRate;

    [Header("Dash")]
    [SerializeField]
    private float dashVelocity;

    [SerializeField]
    private float dashTime;

    [SerializeField]
    private float dashOxygenCost;
    private float dashTimer;
    private bool isDashing;

    private Rigidbody2D rb;
    private PlayerStats pStats;
    private InputAction moveAction;
    private Vector2 moveValue;
    private InputAction dashAction;
    private Vector2 currentDirection;
    private Vector2 lastMoveValue;
    public Vector2 CurrentDirection
    {
        get => currentDirection;
        set => currentDirection = value;
    }

    private bool canMove;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    void Awake()
    {
        DebugTool.EnableLogging(nameof(PlayerController), enableDebug);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pStats = PlayerStats.GlobalPlayerStats;
        rb.gravityScale = gravity;
        canMove = true;
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Dash");
    }

    // Update is called once per frame
    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        if (moveValue != Vector2.zero)
        {
            lastMoveValue = moveValue;
        }

        if (
            dashAction.WasPressedThisFrame()
            && dashTimer <= 0
            && pStats.CurrentOxygenLevel > dashOxygenCost
        )
        {
            Dash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                rb.gravityScale = gravity;
            }
        }
    }

    private void FixedUpdate()
    {
        //Force Application
        if (moveValue != Vector2.zero && CanMove)
        {
            if (Mathf.Abs(moveValue.x) > 0)
            {
                rb.AddForceX(moveValue.x * horizontalAcceleration, ForceMode2D.Force);
            }

            if (moveValue.y > 0)
            {
                DebugTool.Log(moveValue.y);
                rb.AddForceY(moveValue.y * ascendAcceleration, ForceMode2D.Force);
            }
            else if (moveValue.y < 0)
            {
                rb.AddForceY(moveValue.y * descendAcceleration, ForceMode2D.Force);
            }
        }

        //Direction

        if (rb.linearVelocityX > 0)
        {
            currentDirection.x = 1;
        }
        else if (rb.linearVelocityX < 0)
        {
            currentDirection.x = -1;
        }

        if (rb.linearVelocityY > 0)
        {
            currentDirection.y = 1;
        }
        else if (rb.linearVelocityY < 0)
        {
            currentDirection.y = -1;
        }

        //Limits
        if (!isDashing && canMove)
        {
            if (Mathf.Abs(rb.linearVelocityX) >= maxHorizontalVelocity)
            {
                rb.linearVelocityX = maxHorizontalVelocity * currentDirection.x;
            }

            if (rb.linearVelocityY >= maxAscendVelocity)
            {
                rb.linearVelocityY = maxAscendVelocity;
            }

            if (moveValue.y < 0 && rb.linearVelocityY <= -maxDescendVelocity)
            {
                rb.linearVelocityY = -maxDescendVelocity;
            }
            else if (moveValue.y >= 0 && rb.linearVelocityY < -maxGravityVelocity)
            {
                if (Mathf.Abs(rb.linearVelocityY) - maxGravityVelocity > 0.1)
                {
                    rb.linearVelocityY *= postDescendSlowDownRate;
                }
                else
                {
                    rb.linearVelocityY = -maxGravityVelocity;
                }
            }
        }
    }

    private void Dash()
    {
        pStats.AddOxygen(-dashOxygenCost);
        isDashing = true;
        rb.gravityScale = 0;
        dashTimer = dashTime;
        rb.linearVelocity = lastMoveValue * dashVelocity;
    }
}
