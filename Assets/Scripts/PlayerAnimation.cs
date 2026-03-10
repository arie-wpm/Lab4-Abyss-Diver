using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float horizontalDeadzone = 0.1f;
    [SerializeField] private float verticalHoldThreshold = 0.25f;
    [SerializeField] private float noInputThreshold = 0.5f;
    private Rigidbody2D rb;
    private Animator animator;
    private InputAction moveAction;
    private Vector2 lastMoveValue;
    private float verticalHoldTimer;
    private float noInputTimer;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed()) {
            if (moveInput.x != 0) {
                lastMoveValue.x = moveInput.x;
                verticalHoldTimer = 0f;
            }
            if (moveInput.x == 0 && moveInput.y != 0) lastMoveValue.y = moveInput.y;
            noInputTimer = 0f;
        }

        bool isRight = false;
        bool isLeft = false;
        bool isUp = false;
        bool isDown = false;

        if (Mathf.Abs(moveInput.y) > 0) verticalHoldTimer += Time.deltaTime;
        else verticalHoldTimer = 0f;

        bool verticalActive = verticalHoldTimer >= verticalHoldThreshold;

        noInputTimer += Time.deltaTime;
        bool noInputActive = noInputTimer >= noInputThreshold;

        if (verticalActive || noInputActive) {
            isUp = moveInput.y > 0;
            isDown = moveInput.y < 0;
            lastMoveValue = moveInput;
        } else {
            isRight = lastMoveValue.x > 0;
            isLeft = lastMoveValue.x < 0;

            if (velocity.x > horizontalDeadzone && lastMoveValue.x > 0) {
                isRight = true;
                isLeft = false;
            }

            if (velocity.x < -horizontalDeadzone && lastMoveValue.x < 0) {
                isLeft = true;
                isRight = false;
            }
        }

        if (velocity == Vector2.zero) lastMoveValue = Vector2.zero;
        SetAnimParams(isRight, isLeft, isUp, isDown);
    }

    void SetAnimParams(bool isRight, bool isLeft, bool isUp, bool isDown) {
        animator.SetBool("isRight", isRight);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isUp", isUp);
        animator.SetBool("isDown", isDown);
    }
}