using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CurrentScript : MonoBehaviour
{
    [SerializeField]
    private float initialCurrentVelocity;

    [SerializeField]
    private float currentAcceleration;

    private Collider2D currentCollider;

    private Vector2 moveDirection;

    [SerializeField]
    private Transform front;

    [SerializeField]
    private Transform back;

    [Header("Debug")]
    [SerializeField]
    private bool enableDebug = false;

    [HideInInspector]
    public bool movingObject;

    private Rigidbody2D targetRigidBody = null;

    void Awake()
    {
        DebugTool.EnableLogging(nameof(CurrentScript), enableDebug);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Gets vector of movement between the front and back markers.
        moveDirection = front.position - back.position;
        DebugTool.Log(moveDirection);
        currentCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (movingObject && targetRigidBody != null)
        {
            MoveObject(targetRigidBody);
        }
    }

    // Update is called once per frame
    void Update() { }

    public void StartMoveObject(Rigidbody2D rb)
    {
        //Sets up player components to be taken over by the water current.
        rb.gravityScale = 0;
        //Checks if current is horizontal or vertical and moves player into the centre of the current.
        if (Mathf.Abs(moveDirection.x) < 0.01f)
        {
            DebugTool.Log("Set PLayer to Current X factor.");
            rb.MovePosition(new Vector2(front.position.x, rb.position.y));
        }
        if (Mathf.Abs(moveDirection.y) < 0.01f)
        {
            DebugTool.Log("Set PLayer to Current Y factor.");
            rb.MovePosition(new Vector2(rb.position.x, front.position.y));
        }
        rb.linearVelocity = Vector2.zero;
        targetRigidBody = rb;
        movingObject = true;
    }

    public void StopMoveObject(Rigidbody2D rb)
    {
        //Resets plaeyr to move again.
        movingObject = false;
        rb.gravityScale = 0.3f;
        targetRigidBody = null;
    }

    public void MoveObject(Rigidbody2D rb)
    {
        //Moves the player.
        rb.AddForce(moveDirection * currentAcceleration, ForceMode2D.Force);
        //rb.linearVelocity += moveDirection * Time.deltaTime * currentAcceleration;
    }
}
