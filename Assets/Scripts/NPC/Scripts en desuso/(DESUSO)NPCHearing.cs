using UnityEngine;
using UnityEngine.AI;

// (WIP) Esta clase permite al NPC oír sonidos e investigarlos
public class NPCHearing : MonoBehaviour
{
    [SerializeField] private float hearingRadius = 6f;

    public bool HeardNoise { get; private set; }
    public Vector3 NoisePosition { get; private set; }

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Llamado desde fuera (jugador, objetos, etc.)
    public void HearNoise(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) <= hearingRadius)
        {
            HeardNoise = true;
            NoisePosition = position;
        }
    }

    // Lógica mientras investiga un sonido
    public void TickInvestigate()
    {
        agent.SetDestination(NoisePosition);

        // Si llega al punto y no ve nada, se calma
        if (agent.remainingDistance <= agent.stoppingDistance)
            HeardNoise = false;
    }
}
