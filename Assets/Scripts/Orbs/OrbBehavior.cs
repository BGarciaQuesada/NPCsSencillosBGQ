using UnityEngine;

public class OrbBehavior : MonoBehaviour
{
    // Al empezar lo cuenta en el total
    private void Start()
    {
        OrbManager.Instance.RegisterOrb();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si toca PLAYER...
        if (other.CompareTag("Player"))
        {
            // Sumar al contador
            OrbManager.Instance.CollectOrb();

            // Destruir el orbe
            Destroy(gameObject);
        }
    }
}
