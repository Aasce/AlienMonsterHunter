using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEditor.Overlays;
using UnityEngine;

namespace Asce.Game.Items
{
    [System.Serializable]
    public class Currency : Item, ISaveable<CurrencySaveData>, ILoadable<CurrencySaveData>
    {
        public Currency(SO_ItemInformation information) : base(information)
        {

        }




        CurrencySaveData ISaveable<CurrencySaveData>.Save()
        {
            ItemSaveData baseData = ((ISaveable<ItemSaveData>)this).Save();
            CurrencySaveData saveData = new();
            saveData.CopyFrom(baseData);

            return saveData;
        }

        void ILoadable<CurrencySaveData>.Load(CurrencySaveData data)
        {
            if (data == null) return;
            ((ILoadable<ItemSaveData>)this).Load(data);
        }

    }
}