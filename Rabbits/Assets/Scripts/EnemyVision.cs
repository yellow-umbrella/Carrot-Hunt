using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float angle;
    [SerializeField] private float range;
    [SerializeField] private int rayCount;
    [SerializeField] private LayerMask obstaclesLayer;
    [SerializeField] private LayerMask playerLayer;

    private Mesh mesh;
    private EnemyMovement enemy;

    private struct Gizmoray
    {
        public Vector2 gizmoStart;
        public Vector2 gizmoDir;
        public Vector2 point;
    }
    private List<Gizmoray> gizmos;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start()
    {
        enemy = GetComponentInParent<EnemyMovement>();
    }

    private void Update()
    {
        float currentAngle = angle / 2;
        float angleIncrease = angle / rayCount;
        Vector3 origin = Vector3.zero;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        int triangleInd = 0;
        gizmos = new List<Gizmoray>();
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 v;
            float angleRad = Mathf.Deg2Rad * currentAngle;
            Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            Vector2 originGlobal = origin + transform.position;
            float phi = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 directionGlobal = Local2Global(direction, phi);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(originGlobal, directionGlobal, range, obstaclesLayer | playerLayer);
            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.TryGetComponent(out PlayerController player) && !player.IsHidden)
            {
                enemy.OnPlayerDetected(player);
            }
            raycastHit2D = Physics2D.Raycast(originGlobal, directionGlobal, range, obstaclesLayer);

            // draw gizmos for rays and points of hiting
            gizmos.Add(new Gizmoray { gizmoStart = originGlobal, gizmoDir = directionGlobal, point = raycastHit2D.point });

            if (raycastHit2D.collider == null)
            {
                v = origin + direction * range;
            }
            else
            {
                v = Local2Global(raycastHit2D.point - originGlobal, -phi);
            }

            vertices[i] = v;

            if (i > 1)
            {
                triangles[triangleInd] = 0;
                triangles[triangleInd + 1] = i - 1;
                triangles[triangleInd + 2] = i;

                triangleInd += 3;
            }

            currentAngle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private Vector2 Local2Global(Vector2 local, float phi)
    {
        return new Vector2(local.x * Mathf.Cos(phi) - local.y * Mathf.Sin(phi),
                            local.x * Mathf.Sin(phi) + local.y * Mathf.Cos(phi));
    }

    private void OnDrawGizmos()
    {
        if (gizmos == null) return;
        Gizmos.color = Color.green;
        gizmos.ForEach(r => { Gizmos.DrawRay(r.gizmoStart, r.gizmoDir); Gizmos.DrawSphere(r.point, .01f); });
    }
}
