using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Vision")]
    [SerializeField] private float viewRadius = 8f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float eyeHeight = 1.6f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Hearing")]
    [SerializeField] private float hearingRadius = 6f;
    private Vector3 noisePosition;
    private bool heardNoise = false;

    [Header("Chase")]
    [SerializeField] private float losePlayerTime = 2f;
    private float timeSinceLastSeen = 0f;
    private bool playerDetected = false;

    [Header("View Cone Visual")]
    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private MeshRenderer viewMeshRenderer;
    [SerializeField] private int viewMeshResolution = 1;

    private Color patrolColor = new Color(0, 1, 0, 0.3f);
    private Color investigateColor = new Color(1, 1, 0, 0.3f);
    private Color chaseColor = new Color(1, 0, 0, 0.3f);

    private Mesh viewMesh;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        GoToNextPatrolPoint();
        UpdateViewColor();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            playerDetected = true;
            timeSinceLastSeen = 0f;
        }

        if (playerDetected)
        {
            ChasePlayer();
            UpdateViewColor();
            return;
        }

        if (heardNoise)
        {
            InvestigateNoise();
            UpdateViewColor();
            return;
        }

        Patrol();
        UpdateViewColor();
    }

    void LateUpdate()
    {
        DrawViewCone();
    }

    #region Vision

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 dirToPlayer = (player.position - origin).normalized;
        float distToPlayer = Vector3.Distance(origin, player.position);

        if (distToPlayer > viewRadius)
            return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Raycast(origin, dirToPlayer, distToPlayer, obstacleMask))
            return false;

        return true;
    }

    struct ViewCastInfo
    {
        public Vector3 point;
        public float distance;

        public ViewCastInfo(Vector3 _point, float _distance)
        {
            point = _point;
            distance = _distance;
        }
    }

    ViewCastInfo ViewCast(float angle)
    {
        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 dir = DirFromAngle(angle);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(hit.point, hit.distance);
        }
        else
        {
            return new ViewCastInfo(origin + dir * viewRadius, viewRadius);
        }
    }

    Vector3 DirFromAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    #endregion

    #region View Cone Mesh

    void DrawViewCone()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * viewMeshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero);

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = -viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo viewCast = ViewCast(angle);

            Vector3 localPoint = transform.InverseTransformPoint(viewCast.point);
            vertices.Add(localPoint);
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices.ToArray();
        viewMesh.triangles = triangles.ToArray();
        viewMesh.RecalculateNormals();
    }

    void UpdateViewColor()
    {
        if (playerDetected)
            viewMeshRenderer.material.color = chaseColor;
        else if (heardNoise)
            viewMeshRenderer.material.color = investigateColor;
        else
            viewMeshRenderer.material.color = patrolColor;
    }

    #endregion

    #region Behaviours

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        timeSinceLastSeen += Time.deltaTime;

        if (timeSinceLastSeen >= losePlayerTime)
        {
            playerDetected = false;
            GoToNextPatrolPoint();
        }
    }

    void InvestigateNoise()
    {
        agent.SetDestination(noisePosition);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            heardNoise = false;
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    #endregion

    #region Hearing

    public void HearNoise(Vector3 noisePos)
    {
        if (Vector3.Distance(transform.position, noisePos) <= hearingRadius)
        {
            heardNoise = true;
            noisePosition = noisePos;
        }
    }

    #endregion
}
