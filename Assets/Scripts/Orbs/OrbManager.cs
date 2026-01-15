using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public static OrbManager Instance;

    public int totalOrbsInLevel { get; private set; }
    public int totalOrbsCollected { get; private set; }
    public bool allCollected = false;

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

    public void Start()
    {
        ResetOrbs();
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
        {
            // El método flecha lo cargaba el demonio, bool de toda la vida.
            Debug.Log("Todos los orbes recogidos");
            allCollected = true;
        }
    }

    // Opcional: resetear al reiniciar la partida
    // [!] Este método se creó en caso de que otros scripts necesiten resetear el conteo de orbes.
    // [!] Actualmente no es el caso pero si hubiese que escalarlo en un futuro, ahí está.
    public void ResetOrbs()
    {
        totalOrbsCollected = 0;
    }
}
