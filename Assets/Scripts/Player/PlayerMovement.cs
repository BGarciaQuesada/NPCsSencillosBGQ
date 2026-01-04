using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
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

    void Update()
    {
        CheckGround();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    // --- INPUTS ---

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        runHeld = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed) jumpPressed = true;
    }


    // --- LÓGICA DE INPUTS ---

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

    // [!] El raycast aquí es basura, paso...
    // Dibuja una esfera, mira hacia abajo y comprueba si está en su radio (solo para groundLayer)
    private void CheckGround()
    {
        float radius = 0.5f; // radio del "pie"
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * 0.9f, radius, groundLayer);
    }

}
