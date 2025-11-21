using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Items
{
    [System.Serializable]
    public class Item : IIdentifiable, ISaveable<ItemSaveData>, ILoadable<ItemSaveData>
    {
        public const string PREFIX_ID = "item_";

        [SerializeField, Readonly] protected string _id;
        [SerializeField, Readonly] protected SO_ItemInformation _information;
        
        [Space]
        [SerializeField] protected int _quantity = 0;

        public event Action<int> OnQuantityChanged;

        public string Id => _id;
        public SO_ItemInformation Information => _information;
        public int Quantity 
        { 
            get => _quantity; 
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnQuantityChanged?.Invoke(_quantity);
            }
        }

        string IIdentifiable.Id 
        { 
            get => Id; 
            set => _id = value; 
        }

        public Item(SO_ItemInformation information) 
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            _information = information;
        }

        ItemSaveData ISaveable<ItemSaveData>.Save()
        {
            ItemSaveData saveData = new ()
            {
                id = _id,
                name = _information.Name,
                quantity = _quantity
            };
            return saveData;
        }

        void ILoadable<ItemSaveData>.Load(ItemSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            _quantity = data.quantity;
        }

    }
}