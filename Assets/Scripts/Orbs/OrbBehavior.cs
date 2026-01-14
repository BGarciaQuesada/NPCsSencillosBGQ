using UnityEngine;

public class OrbBehavior : MonoBehaviour
{

    [SerializeField] private AudioClip orbCollected;

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
            if (orbCollected != null)
            {
                Debug.Log("El sonido se ha encontrado");

                // Si intento hacer esto al destruirse el objeto pues... también el sonido
                // OrbCollected.Play();

                // Solucón, audioclip directamente:
                AudioSource.PlayClipAtPoint(
                orbCollected,
                transform.position
                );
            }

            // Sumar al contador
            OrbManager.Instance.CollectOrb();

            // Destruir el orbe
            Destroy(gameObject);
        }
    }
}
