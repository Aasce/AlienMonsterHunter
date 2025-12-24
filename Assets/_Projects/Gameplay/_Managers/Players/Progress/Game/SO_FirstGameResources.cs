using Asce.Core;
using Asce.Game.Progress;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{ 
    [CreateAssetMenu(menuName = "Asce/Progress/First Game Resources", fileName = "First Game Resources")]
    public class SO_FirstGameResources : ScriptableObject
    {
        [SerializeField] private ListObjects<string, CustomValue> _items = new ((custom) =>
        {
            return custom.Name;
        });

        [SerializeField] private SO_UnlockFirstGameStart _firstGameStartUnlock;

        public ReadOnlyCollection<CustomValue> Items => _items.List;
        public CustomValue Get(string name) => _items.Get(name);
        
        public SO_UnlockFirstGameStart FirstGameStartUnlock => _firstGameStartUnlock;
    }
}