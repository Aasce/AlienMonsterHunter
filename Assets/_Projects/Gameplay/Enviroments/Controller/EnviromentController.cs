using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Enviroments
{
    public class EnviromentController : MonoBehaviourSingleton<EnviromentController>
    {
        [SerializeField] private Bounds _mapBounds = new Bounds(Vector3.zero, new Vector3(20f, 20f, 0f));

        [Space]
        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private Transform _supportSpawnPoint;

        /// <summary> Map bounds in world space. </summary>
        public Bounds MapBounds => _mapBounds;

        public Vector2 SupportSpawnPoint => _supportSpawnPoint != null ? _supportSpawnPoint.position : Vector2.zero;
        public Vector2 CharacterSpawnPoint => _characterSpawnPoint != null ? _characterSpawnPoint.position : Vector2.zero;
        

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_mapBounds.center, _mapBounds.size);
        }
#endif
    }
}
