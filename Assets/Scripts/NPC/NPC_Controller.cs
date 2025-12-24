using UnityEngine;
using UnityEngine.AI;

// Esta clase maneja los estados y llama a los métodos de las otras clases NPC

public class NPC_Controller : MonoBehaviour
{
    public enum NPCState
    {
        Patrol,   
        Investigate,  
        Chase          
    }

    public NPCState currentState;

    // Referencias a otros componentes del NPC
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public NPC_Patrol patrol;
    [HideInInspector] public NPC_Vision vision;
    [HideInInspector] public NPC_Hearing hearing;

    // Tiempo que puede perder de vista al jugador antes de rendirse
    [SerializeField] private float losePlayerTime = 2f;
    private float timeSinceLastSeen;

    void Awake()
    {
        // Obtener referencias automáticamente
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<NPC_Patrol>();
        vision = GetComponent<NPC_Vision>();
        hearing = GetComponent<NPC_Hearing>();
    }

    void Start()
    {
        currentState = NPCState.Patrol;
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Patrol:
                patrol.Tick();

                // Prioridad: ver al jugador > oír ruido
                if (vision.CanSeePlayer)
                    EnterChase();
                else if (hearing.HeardNoise)
                    EnterInvestigate();
                break;

            case NPCState.Investigate:
                hearing.TickInvestigate();

                if (vision.CanSeePlayer)
                    EnterChase();
                else if (!hearing.HeardNoise)
                    EnterPatrol();
                break;

            case NPCState.Chase:
                ChasePlayer();
                break;
        }
    }

    // ---- TRANSICIONES DE ESTADO ----

    void EnterPatrol()
    {
        currentState = NPCState.Patrol;
        patrol.GoToNextPoint();
    }

    void EnterInvestigate()
    {
        currentState = NPCState.Investigate;
        agent.SetDestination(hearing.NoisePosition);
    }

    void EnterChase()
    {
        currentState = NPCState.Chase;
        timeSinceLastSeen = 0f;
    }

    // ---- PERSECUCIÓN ----

    void ChasePlayer()
    {
        agent.SetDestination(vision.Player.position);

        if (!vision.CanSeePlayer)
        {
            // Si no lo ve, empieza a contar
            timeSinceLastSeen += Time.deltaTime;

            if (timeSinceLastSeen >= losePlayerTime)
                EnterPatrol();
        }
        else
        {
            // Si lo vuelve a ver, resetea el contador
            timeSinceLastSeen = 0f;
        }
    }
}
