using Asce.Game.Items;
using Asce.Game.Players;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.SaveLoads
{
    public class SaveLoadPlayerItemsController : SaveLoadController
    {
        protected override void LoadName()
        {
            _name = "Player Items";
        }

        public void SaveAllItems()
        {
            foreach (Item item in PlayerManager.Instance.Items.AllItems)
            {
                SaveItem(item);
            }
        }

        public void SaveItem(Item item)
        {
            if (item == null) return;
            if (item is not ISaveable<ItemSaveData> saveable) return;
            ItemSaveData saveData = saveable.Save();

            SaveLoadManager.Instance.SaveIntoFolder("Items", item.Information.Name, saveData);
        }

        public void LoadItem(Item item)
        {
            if (item == null) return;
            if (item is not ILoadable<ItemSaveData> loadable) return;

            ItemSaveData saveData = SaveLoadManager.Instance.LoadFromFolder<ItemSaveData>("Items", item.Information.Name);
            loadable.Load(saveData);
        }

    }
}