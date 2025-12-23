using Asce.Core;
using Asce.Game.Enviroments;
using UnityEngine;

namespace Asce.Game.Maps
{
    public class Map : GameComponent
    {
        [SerializeField] private SO_MapInformation _information;
        [SerializeField] private SpawnPoints _spawnPoints;

        public SO_MapInformation Information => _information;
        public SpawnPoints SpawnPoints => _spawnPoints;

    }
}