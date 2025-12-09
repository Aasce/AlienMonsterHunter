using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Spawners
{
    [CreateAssetMenu(menuName = "Asce/Spawners/All Spawners", fileName = "All Spawners")]
    public class SO_AllSpawners : ScriptableObject
    {
        [SerializeField] private ListObjects<string, BaseSpawner> _spawners = new((spawner) =>
        {
            if (spawner == null) return null;
            return spawner.Name;
        });

        public ReadOnlyCollection<BaseSpawner> Spawners => _spawners.List;
        public BaseSpawner Get(string name) => _spawners.Get(name);
        public T Get<T>(string name) where T :BaseSpawner => _spawners.Get(name) as T;

    }
}