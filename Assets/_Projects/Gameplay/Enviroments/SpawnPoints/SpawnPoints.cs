using Asce.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Enviroments
{
    public class SpawnPoints : GameComponent
    {
        [SerializeField] private Transform _characterSpawnPoint;
        [SerializeField] private Transform _supportSpawnPoint;

        [Space]
        [SerializeField] private List<PlaceHolder> _spawnHolders = new();

        public Vector2 SupportSpawnPoint => _supportSpawnPoint != null ? _supportSpawnPoint.position : Vector2.zero;
        public Vector2 CharacterSpawnPoint => _characterSpawnPoint != null ? _characterSpawnPoint.position : Vector2.zero;

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
    }
}