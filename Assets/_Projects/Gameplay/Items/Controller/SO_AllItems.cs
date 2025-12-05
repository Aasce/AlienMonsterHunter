using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Items
{
    [CreateAssetMenu(menuName = "Asce/Items/All Items", fileName = "All Items")]
    public class SO_AllItems : ScriptableObject
    {
        [SerializeField] private ListObjects<string, SO_ItemInformation> _currencies = new ((currency) =>
        {
            if (currency == null) return null;
            return currency.Name;
        });

        public ReadOnlyCollection<SO_ItemInformation> Currencies => _currencies.List;


        public SO_ItemInformation GetCurrency(string name) => _currencies.Get(name);
    }
}