using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float groundDrag;
    public movementState state;
    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // Data inputs.
    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    // Movement states.
    public enum movementState
    {
        walking,
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Ground check.
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.05f,
            whatIsGround
        );

        // Handle drag.
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        // Get player input and handle the state.
        MyInput();
        StateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // Get player inputs.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate the movement direction.
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Move player on ground.
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 100f, ForceMode.Force);
        }
    }

    private void StateHandler()
    {
        // Mode - Walking
        if (grounded)
        {
            state = movementState.walking;
            moveSpeed = walkSpeed;
        }
    }
}
