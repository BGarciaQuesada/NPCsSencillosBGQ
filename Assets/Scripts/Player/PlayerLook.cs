using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Ajustes de Cámara 1ªP")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float maxLookAngle = 60f;

    private Vector2 lookInput;
    private float xRotation = 0f;

    void Update()
    {
        HandleLook();
    }

    // --- INPUT ---
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        // Rotación vertical (cámara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (player entero)
        transform.Rotate(Vector3.up * mouseX);
    }
}
