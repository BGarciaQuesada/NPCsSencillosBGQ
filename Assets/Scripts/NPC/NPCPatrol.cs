using UnityEngine;
using UnityEngine.AI;

// Esta clase controla el movimiento del NPC entre puntos de patrulla (acción por defecto antes de perseguir al jugador)

public class NPCPatrol : MonoBehaviour
{
    // [!] Por el amor de jesucristo asignale los scripts a las cosas belen

    [SerializeField] private Transform[] patrolPoints;
    private int currentIndex;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Tick();
    }


    // Se llama cada frame mientras el NPC patrulla
    public void Tick()
    {
        // Si no hay puntos de patrulla, no hacer nada
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Si ha llegado al destino (o está muy cerca)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPoint();
        }
    }

    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
        currentIndex = 0;

        Debug.Log($"{name} recibió {patrolPoints?.Length ?? 0} patrol points");

        // Si no hay puntos de patrulla, nada
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        // Ir al primer punto inmediatamente
        agent.SetDestination(patrolPoints[currentIndex].position);
    }

    public void GoToNextPoint()
    {
        // Si no hay puntos de patrulla, nada
        if (patrolPoints.Length == 0) return;

        // [!] Antes me ha dado problemas por estar en el otro orden pero si lo pongo así va bien asi que...
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentIndex].position);
        
    }
}
