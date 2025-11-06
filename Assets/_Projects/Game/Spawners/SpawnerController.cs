using Asce.Game.Spawners;
using Asce.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class SpawnerController : GameComponent
    {
        [SerializeField] private List<BaseSpawner> _spawners = new();

        public List<BaseSpawner> Spawners => _spawners;

        public void Initialize()
        {
            foreach (BaseSpawner spawner in _spawners) 
            {
                spawner.Initialize();
            }
        }

        public void OnCreate()
        {
            foreach (BaseSpawner spawner in _spawners)
            {
                spawner.OnCreate();
            }
        }

        public void OnLoad()
        {
            foreach (BaseSpawner spawner in _spawners)
            {
                spawner.OnLoad();
            }
        }

        public void StartSpawn()
        {
            foreach (BaseSpawner spawner in _spawners)
            {
                spawner.Active = true;
            }
        }

        public void StopSpawn()
        {
            foreach (BaseSpawner spawner in _spawners)
            {
                spawner.Active = false;
            }
        }
    }
}