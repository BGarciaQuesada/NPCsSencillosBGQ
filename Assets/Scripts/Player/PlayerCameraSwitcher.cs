using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private Camera[] cameras;

    private int currentIndex = 0;

    void Start()
    {
        // Activar solo la primera cámara
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentIndex);
        }
    }

    // Este método lo llamaremos desde PlayerInput
    public void OnSwitchCamera(InputValue value)
    {
        if (value.isPressed)
        {
            // Apagar la cámara actual
            cameras[currentIndex].gameObject.SetActive(false);

            // Siguiente cámara
            currentIndex = (currentIndex + 1) % cameras.Length;

            // Activar la nueva
            cameras[currentIndex].gameObject.SetActive(true);
        }
    }
}
