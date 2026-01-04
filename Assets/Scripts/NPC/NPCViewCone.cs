using System.Collections.Generic;
using UnityEngine;


// Esta clase se encarga de DIBUJAR el cono de visión visible en escena. NO DETECTA, solo representa.
public class NPCViewCone : MonoBehaviour
{
    // [!] Por el amor de jesucristo asignale los scripts a las cosas belen

    [SerializeField] private NPCVision vision;
    [SerializeField] private NPCController controller;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private int resolution = 1;

    private Color patrolColor = new(0, 1, 0, 0.3f);
    private Color investigateColor = new(1, 1, 0, 0.3f);
    private Color chaseColor = new(1, 0, 0, 0.3f);

    private Mesh mesh;

    void Awake()
    {
        // Crear mesh procedural
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    void LateUpdate()
    {
        DrawCone();
        UpdateColor();
    }

    void DrawCone()
    {
        int steps = Mathf.RoundToInt(vision.ViewAngle * resolution);
        float stepAngle = vision.ViewAngle / steps;

        List<Vector3> vertices = new() { Vector3.zero };
        List<int> triangles = new();

        Vector3 origin = vision.transform.position + Vector3.up * vision.EyeHeight;

        for (int i = 0; i <= steps; i++)
        {
            float angle = -vision.ViewAngle / 2 + stepAngle * i;
            Vector3 dir = DirFromAngle(angle);

            // Raycast para cortar el cono con paredes
            if (Physics.Raycast(origin, dir, out RaycastHit hit,
                vision.ViewRadius, vision.ObstacleMask))
            {
                vertices.Add(transform.InverseTransformPoint(hit.point));
            }
            else
            {
                vertices.Add(dir * vision.ViewRadius);
            }
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    Vector3 DirFromAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void UpdateColor()
    {
        meshRenderer.material.color = controller.currentState switch
        {
            NPCController.NPCState.Patrol => patrolColor,
            NPCController.NPCState.Investigate => investigateColor,
            NPCController.NPCState.Chase => chaseColor,
            _ => patrolColor // (= Cualquier otro caso en un switch C#)
        };
    }
}
