using UnityEngine;

namespace Asce.Game.Progress
{
    [System.Serializable]
    public class Award
    {
        [SerializeField] private string _itemName = string.Empty;
        [SerializeField, Min(0)] private int _quantity = 0;

        public string ItemName => _itemName;
        public int Quantity => _quantity;

        public Award(string itemName, int quantity = 0)
        {
            _itemName = itemName;
            _quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            _quantity += quantity;
        }
    }
}