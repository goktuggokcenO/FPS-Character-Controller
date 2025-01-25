using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public movementState state;
    public Transform orientation;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool isReadyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // Data inputs.
    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    // Movement states.
    public enum movementState
    {
        walking,
        sprinting,
        air,
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        isReadyToJump = true;
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
        SpeedControl();
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

        // Jump if pressed to jump key.
        if (Input.GetKey(jumpKey) && isReadyToJump && grounded)
        {
            isReadyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void StateHandler()
    {
        // Mode - Sprinting
        if (grounded && Input.GetKey(sprintKey))
        {
            state = movementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            state = movementState.walking;
            moveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            state = movementState.air;
        }
    }

    private void MovePlayer()
    {
        // Calculate the movement direction.
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Move player on ground.
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 1000f, ForceMode.Force);
        }
        // Move player on air.
        else
        {
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 1000f * airMultiplier,
                ForceMode.Force
            );
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce * 100f, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private void SpeedControl()
    {
        // Liimit the player speed.
        Vector3 flatMoveVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatMoveVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVeocity = flatMoveVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(
                limitedVeocity.x,
                rb.linearVelocity.y,
                limitedVeocity.z
            );
        }
    }
}
