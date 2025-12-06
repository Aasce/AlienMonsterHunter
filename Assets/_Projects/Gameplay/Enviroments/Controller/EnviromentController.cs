using Asce.Core;
using UnityEngine;

namespace Asce.Game.Enviroments
{
    public class EnviromentController : MonoBehaviourSingleton<EnviromentController>
    {
        [SerializeField] private Bounds _mapBounds = new Bounds(Vector3.zero, new Vector3(20f, 20f, 0f));

        [Space]
        [SerializeField] private SpawnPoints _spawnPoints;

        /// <summary> Map bounds in world space. </summary>
        public Bounds MapBounds => _mapBounds;
        public SpawnPoints SpawnPoints => _spawnPoints;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_mapBounds.center, _mapBounds.size);
        }
#endif
    }
}
