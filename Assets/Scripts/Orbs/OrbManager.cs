using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Instance;

    public int totalOrbsInLevel { get; private set; }
    public int totalOrbsCollected { get; private set; }

    // Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Llamado cuando un orbe APARECE en el mapa
    public void RegisterOrb()
    {
        totalOrbsInLevel++;
        // Debug.Log($"Orbes totales actuales: {totalOrbsInLevel}");
    }

    // Llamado cuando el jugador RECOGE un orbe
    public void CollectOrb()
    {
        totalOrbsCollected++;
        // Debug.Log($"Orbes totales actuales: {totalOrbsCollected}");

        if (totalOrbsCollected >= totalOrbsInLevel)
            Debug.Log("Todos los orbes recogidos");
    }

    // Método flecha booleana que devuelve si los coleccionados >= totales
    public bool AllCollected => totalOrbsCollected >= totalOrbsInLevel;

    // Opcional: resetear al reiniciar la partida
    public void ResetOrbs()
    {
        totalOrbsCollected = 0;
    }
}
