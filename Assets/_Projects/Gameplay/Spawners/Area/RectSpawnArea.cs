using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    [AddComponentMenu("Asce/Spawners/Spawn Areas/Rect Spawn Area")]
    public class RectSpawnArea : SpawnArea
    {
        [Header("Area Bounds")]
        [SerializeField] private Vector2 _areaX = new(-37.5f, 37.5f);
        [SerializeField] private Vector2 _areaY = new(-25f, 25f);

        [Header("NavMesh Sampling")]
        [SerializeField] private float _samplePositionMaxDistance = 0.5f;
        [SerializeField] private int _maxAttempts = 10;

        public override bool TryGetSpawnPosition(NavMeshQueryFilter filter, out Vector3 position)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                float x = Random.Range(_areaX.x, _areaX.y);
                float y = Random.Range(_areaY.x, _areaY.y);
                Vector3 candidate = new(x, y, 0);

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _samplePositionMaxDistance, filter))
                {
                    position = hit.position;
                    return true;
                }
            }

            position = Vector3.zero;
            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new((_areaX.x + _areaX.y) / 2f, (_areaY.x + _areaY.y) / 2f, 0);
            Vector3 size = new(_areaX.y - _areaX.x, _areaY.y - _areaY.x, 0);
            Gizmos.DrawWireCube(center, size);
        }
#endif
    }
}
