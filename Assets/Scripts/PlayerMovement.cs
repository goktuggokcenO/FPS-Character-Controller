using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public movementState state;
    public Transform orientation;

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

        // Move player.
        rb.AddForce(moveDirection.normalized * moveSpeed * 100f, ForceMode.Force);
    }

    private void StateHandler()
    {
        // Mode - Walking
        state = movementState.walking;
        moveSpeed = walkSpeed;
    }
}
