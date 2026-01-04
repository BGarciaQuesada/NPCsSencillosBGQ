using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float airControlMultiplier = 0.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;

    // Inputs
    private Vector2 moveInput;
    private bool runHeld;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // --- MOVIMIENTOS ---

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runHeld = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpPressed = true;
    }

    // ----------------------

    void Update()
    {
        CheckGround();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        float speed = runHeld ? runSpeed : walkSpeed;

        Vector3 moveDir = transform.TransformDirection(inputDir);
        float control = isGrounded ? 1f : airControlMultiplier;

        Vector3 horizontalVelocity = moveDir * speed * control;

        rb.linearVelocity = new Vector3(
            horizontalVelocity.x,
            rb.linearVelocity.y,
            horizontalVelocity.z
        );
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpPressed = false;
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }
}
