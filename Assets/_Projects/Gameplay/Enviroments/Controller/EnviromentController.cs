using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.Maps;
using UnityEngine;

namespace Asce.Game.Enviroments
{
    public class EnviromentController : MonoBehaviourSingleton<EnviromentController>
    {
        [SerializeField] private Map _map;
        [SerializeField] private Transform _filter;

        [Space]
        [SerializeField] private Bounds _mapBounds = new Bounds(Vector3.zero, new Vector3(20f, 20f, 0f));


        public Map Map => _map;
        public Transform Filter => _filter;
        public SpawnPoints SpawnPoints => Map.SpawnPoints;

        /// <summary> Map bounds in world space. </summary>
        public Bounds MapBounds => _mapBounds;

        public void SetMap(string mapName)
        {
            Map mapPrefab = GameManager.Instance.AllMaps.Get(mapName);
            Map map = Instantiate(mapPrefab);

            _map = map;
            _mapBounds.extents = new Vector3(map.Information.MapSize.x, map.Information.MapSize.y, 0f);
            _filter.localScale = new Vector3(map.Information.MapSize.x * 2f, map.Information.MapSize.y * 2f, 1f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_mapBounds.center, _mapBounds.size);
        }
#endif
    }
}
