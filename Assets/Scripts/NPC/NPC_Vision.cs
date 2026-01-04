using UnityEngine;


// Esta clase gestiona la LÓGICA de la visión del NPC (cono + obstáculos). NO DIBUJA, solo detecta.
public class NPC_Vision : MonoBehaviour
{
    // [!] Por el amor de jesucristo asignale los scripts a las cosas belen

    [SerializeField] private Transform player;
    [SerializeField] private float viewRadius = 8f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float eyeHeight = 1.6f;
    [SerializeField] private LayerMask obstacleMask;

    public bool CanSeePlayer { get; private set; }

    // Getter de Player para que otros scripts puedan acceder (controller)
    public Transform Player => player;

    void Update()
    {
        CanSeePlayer = CheckVision();
    }

    bool CheckVision()
    {
        // Origen del raycast (subir a los "ojos" del NPC)
        Vector3 origin = transform.position + Vector3.up * eyeHeight;

        Vector3 dirToPlayer = (player.position - origin).normalized;
        float distance = Vector3.Distance(origin, player.position);

        // Fuera de rango...
        if (distance > viewRadius)
            return false;

        // Fuera del ángulo...
        if (Vector3.Angle(transform.forward, dirToPlayer) > viewAngle / 2f)
            return false;

        // Obstáculo entre medias...
        if (Physics.Raycast(origin, dirToPlayer, distance, obstacleMask))
            return false;

        // Si no se cumple nada de lo anterior, lo ve
        return true;
    }

    // Getters para ViewCone
    public float ViewRadius => viewRadius;
    public float ViewAngle => viewAngle;
    public float EyeHeight => eyeHeight;
    public LayerMask ObstacleMask => obstacleMask;
}
