using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Core;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerItems : GameComponent
    {
        [SerializeField] protected List<Item> _items = new();
        protected Dictionary<string, Item> _itemsDictionary;
        protected ReadOnlyCollection<Item> _itemsReadonly;

        public ReadOnlyCollection<Item> AllItems
        {
            get
            {
                if (_itemsReadonly == null) this.Initialize();
                return _itemsReadonly;
            }
        }
        public SaveLoadPlayerItemsController SaveLoadItemsController => SaveLoadManager.Instance.GetController("Player Items") as SaveLoadPlayerItemsController;

        public virtual void Initialize()
        {
            foreach (SO_ItemInformation information in GameManager.Instance.AllItems.Items)
            {
                Item item = new(information);
                SaveLoadItemsController.LoadItem(item);
                _items.Add(item);
            }

            _itemsDictionary = new Dictionary<string, Item>();
            foreach (Item item in _items)
            {
                _itemsDictionary[item.Information.Name] = item;
            }

            _itemsReadonly = _items.AsReadOnly();
        }

        public Item Get(string name)
        {
            if (_itemsDictionary == null) this.Initialize();
            if (_itemsDictionary.TryGetValue(name, out Item item))
            {
                return item;
            }
            return null;
        }

        public void Add(string name, int amount)
        {
            if (amount <= 0) return;
            Item item = this.Get(name);
            if (item == null) return;

            item.Quantity += amount;
            SaveLoadItemsController.SaveItem(item);
        }

        public bool TrySpend(string name, int amount)
        {
            if (amount <= 0) return true;
            Item item = this.Get(name);
            if (item == null) return false;

            if (item.Quantity < amount) return false;
            item.Quantity -= amount;

            SaveLoadItemsController.SaveItem(item);
            return true;
        }

        public bool CanSpend(string name, int amount)
        {
            if (amount <= 0) return true;
            Item item = this.Get(name);
            if (item == null) return false;
            return item.Quantity >= amount;
        }


        public void SaveAll() 
        {
            if (SaveLoadItemsController == null) return;
            SaveLoadItemsController.SaveAllItems();
        }
    }
}
