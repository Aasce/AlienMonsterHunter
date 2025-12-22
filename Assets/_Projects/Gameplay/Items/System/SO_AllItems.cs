using Asce.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Items
{
    [CreateAssetMenu(menuName = "Asce/Items/All Items", fileName = "All Items")]
    public class SO_AllItems : ScriptableObject
    {
        [SerializeField] private ListObjects<string, SO_ItemInformation> _items = new ((item) =>
        {
            if (item == null) return null;
            return item.Name;
        });

        public ReadOnlyCollection<SO_ItemInformation> Items => _items.List;

        public SO_ItemInformation Get(string name) => _items.Get(name);
    }
}