using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Supports
{
    [CreateAssetMenu(menuName = "Asce/Supports/All Supports", fileName = "All Supports")]
    public class SO_AllSupports : ScriptableObject
    {
        [SerializeField] private ListObjects<string, Support> _supports = new((support) =>
        {
            if (support == null) return null;
            if (support.Information == null) return null;
            return support.Information.Id;
        });

        public ReadOnlyCollection<Support> Supports => _supports.List;
        public Support Get(string id) => _supports.Get(id);
    }
}
