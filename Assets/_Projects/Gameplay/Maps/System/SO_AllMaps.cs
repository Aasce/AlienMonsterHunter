using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Maps
{
    [CreateAssetMenu(menuName = "Asce/Maps/All Maps", fileName = "All Maps")]
    public class SO_AllMaps : ScriptableObject
    {
        [SerializeField]
        private ListObjects<string, Map> _maps = new((map) =>
        {
            if (map == null) return null;
            if (map.Information == null) return null;
            return map.Information.Name;
        });

        public ReadOnlyCollection<Map> Maps => _maps.List;
        public Map GetMap(string name) => _maps.Get(name);
    }
}
