using Asce.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    public abstract class SpawnArea : GameComponent
    {
        /// <summary>
        /// Returns a valid spawn position within this area.
        /// </summary>
        public abstract bool TryGetSpawnPosition(NavMeshQueryFilter filter, out Vector3 position);
    }
}
