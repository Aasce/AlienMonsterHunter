using Asce.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Enviroments
{
    public class SpawnPoints : GameComponent
    {
        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private Transform _supportSpawnPoint;


        [Header("Enemies Area")]
        [SerializeField] private Vector2 _areaX = new(-37.5f, 37.5f);
        [SerializeField] private Vector2 _areaY = new(-25f, 25f);

        [Space]
        [SerializeField] private List<PlaceHolder> _spawnHolders = new();

        public Vector2 SupportSpawnPoint => _supportSpawnPoint != null ? _supportSpawnPoint.position : Vector2.zero;
        public Vector2 CharacterSpawnPoint => _characterSpawnPoint != null ? _characterSpawnPoint.position : Vector2.zero;

        public Vector2 GetRandomPointInArea()
        {
            float x = Random.Range(_areaX.x, _areaX.y);
            float y = Random.Range(_areaY.x, _areaY.y);
            return new Vector2(x, y);
        }

        public Vector2 GetEmptyHolder()
        {
            foreach (PlaceHolder holder in _spawnHolders) 
            {
                if (holder == null) continue;
                if (holder.Point == null) continue;
                if (holder.IsOccupied) continue;

                holder.IsOccupied = true;
                return holder.Point.position;
            }

            return Vector2.zero;
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