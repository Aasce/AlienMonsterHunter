using Asce.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Items
{
    public class SpoilsController : MonoBehaviourSingleton<SpoilsController>
    {
        [SerializeField] private Dictionary<string, int> _spoils = new();

        public Dictionary<string, int> Spoils => _spoils;

        public void AddSpoil(string name, int quantity)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (quantity <= 0) return;
            if (_spoils.ContainsKey(name))
            {
                _spoils[name] += quantity;
            }
            else
            {
                _spoils.Add(name, quantity);
            }
        }
    }
}
