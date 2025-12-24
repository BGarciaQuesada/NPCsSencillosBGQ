using UnityEngine;
using UnityEngine.AI;

// Esta clase controla el movimiento del NPC entre puntos de patrulla (acción por defecto antes de perseguir al jugador)

public class NPC_Patrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    private int currentIndex;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Se llama cada frame mientras el NPC patrulla
    public void Tick()
    {
        // Si ha llegado al destino, va al siguiente punto
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            GoToNextPoint();
    }

    public void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentIndex].position);
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
    }
}
