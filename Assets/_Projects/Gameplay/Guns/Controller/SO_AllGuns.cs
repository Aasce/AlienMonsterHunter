using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Guns
{
    [CreateAssetMenu(menuName = "Asce/Guns/All Guns", fileName = "All Guns")]
    public class SO_AllGuns : ScriptableObject
    {
        [SerializeField]
        private ListObjects<string, Gun> _guns = new((gun) => {
            if (gun == null) return null;
            if (gun.Information == null) return null;
            return gun.Information.Name;
        });

        public ReadOnlyCollection<Gun> Guns => _guns.List;
        public Gun Get(string name) => _guns.Get(name);
    }
}
