using Asce.Game.Enviroments;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    [AddComponentMenu("Asce/Spawners/Spawn Areas/Rect Spawn Area")]
    public class RectSpawnArea : SpawnArea
    {
        [Header("NavMesh Sampling")]
        [SerializeField] private float _samplePositionMaxDistance = 0.5f;
        [SerializeField] private int _maxAttempts = 10;

        public override bool TryGetSpawnPosition(NavMeshQueryFilter filter, out Vector3 position)
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                Vector3 candidate = EnviromentController.Instance.SpawnPoints.GetRandomPointInArea();

                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _samplePositionMaxDistance, filter))
                {
                    position = hit.position;
                    return true;
                }
            }

            position = Vector3.zero;
            return false;
        }
    }
}
