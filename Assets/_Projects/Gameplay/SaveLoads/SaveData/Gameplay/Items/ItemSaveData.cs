using Asce.SaveLoads;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class ItemSaveData : SaveData
    {
        public string id;
        public string name;
        public int quantity;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is ItemSaveData itemData)
            {
                id = itemData.id;
                name = itemData.name;
                quantity = itemData.quantity;
            }
        }
    }
}