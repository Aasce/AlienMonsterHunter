using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.FOVs
{
    public class FieldOfView : GameComponent
    {
        [SerializeField] private MeshFilter _viewMeshFilter;
        private Mesh _viewMesh;

        [Space]
        [SerializeField] private float _viewRadius = 5f;
        [SerializeField][Range(0, 360)] private float _viewAngle = 90f;
        [SerializeField] private float _meshResolution = 1f;
        [SerializeField] private int _edgeResolveIterations = 4;
        [SerializeField] private float _edgeDistanceThreshold = 0.5f;
        [SerializeField] private LayerMask _obstacleMask;

        // Cached
        private readonly List<Vector3> _viewPoints = new();

        public Mesh ViewMesh => _viewMesh;
        public float ViewRadius => _viewRadius;
        public float ViewAngle => _viewAngle;
        public LayerMask ObstacleMask => _obstacleMask;
        public float MeshResolution => _meshResolution;
        public int EdgeResolveIterations => _edgeResolveIterations;
        public float EdgeDistanceThreshold => _edgeDistanceThreshold;

        protected virtual void Awake()
        {
            _viewMesh = new Mesh
            {
                name = "View Mesh"
            };
            _viewMeshFilter.mesh = _viewMesh;
        }

        public virtual void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(_viewAngle * _meshResolution);
            float stepAngleSize = _viewAngle / stepCount;

            _viewPoints.Clear();
            ViewCastInfo oldViewCast = new();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.z - _viewAngle * 0.5f + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > _edgeDistanceThreshold;
                    if (oldViewCast.isHit != newViewCast.isHit || (oldViewCast.isHit && newViewCast.isHit && edgeThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if (edge.pointA != Vector2.zero) _viewPoints.Add(edge.pointA);
                        if (edge.pointB != Vector2.zero) _viewPoints.Add(edge.pointB);
                    }
                }

                _viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = _viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;

            for (int i = 0; i < _viewPoints.Count; i++)
            {
                // Local space
                vertices[i + 1] = transform.InverseTransformPoint(_viewPoints[i]);

                if (i < _viewPoints.Count - 1)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 2;
                    triangles[i * 3 + 2] = i + 1;
                }
            }

            _viewMesh.Clear();
            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
            _viewMesh.RecalculateBounds();
        }

        protected virtual ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _viewRadius, _obstacleMask);

            if (hit) return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            return new ViewCastInfo(false, (Vector3)transform.position + dir * _viewRadius, _viewRadius, globalAngle);
        }

        protected EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector2 minPoint = Vector2.zero;
            Vector2 maxPoint = Vector2.zero;

            for (int i = 0; i < _edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > _edgeDistanceThreshold;

                if (newViewCast.isHit == minViewCast.isHit && !edgeThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }
            return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
