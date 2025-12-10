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
        private ReadOnlyCollection<SO_ItemInformation> _currenciesReadonly;

        public ReadOnlyCollection<SO_ItemInformation> Items => _items.List;
        public ReadOnlyCollection<SO_ItemInformation> Currencies => _currenciesReadonly ??= this.CreateCurrencies();

        public SO_ItemInformation Get(string name) => _items.Get(name);


        private ReadOnlyCollection<SO_ItemInformation> CreateCurrencies()
        {
            List<SO_ItemInformation> currencies = new();
            foreach (var item in _items.List)
            {
                if (item == null) continue;
                if (item.Type != ItemType.Currency) continue;
                currencies.Add(item);
            }
            return currencies.AsReadOnly();
        }

    }
}