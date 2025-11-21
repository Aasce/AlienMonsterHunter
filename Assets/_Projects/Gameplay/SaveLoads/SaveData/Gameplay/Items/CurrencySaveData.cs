using Asce.SaveLoads;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class CurrencySaveData : ItemSaveData
    {

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CurrencySaveData currencyData)
            {

            }
        }
    }
}