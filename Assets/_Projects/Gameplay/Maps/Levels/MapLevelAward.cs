using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Maps
{
    [System.Serializable]
    public class MapLevelAward
    {
        [SerializeField] private string _itemName = string.Empty;
        [SerializeField, Min(0)] private int _quantity = 0;

        public string ItemName => _itemName;
        public int Quantity => _quantity;
    }
}
