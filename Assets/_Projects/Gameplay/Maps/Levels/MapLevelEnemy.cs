using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Maps
{
    [System.Serializable]
    public class MapLevelEnemy
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField, Min(0)] private int _level = 0;
        [SerializeField, Min(0)] private int _quantity = 0;

        public string Name => _name;
        public int Level => _level;
        public int Quantity => _quantity;
    }
}
