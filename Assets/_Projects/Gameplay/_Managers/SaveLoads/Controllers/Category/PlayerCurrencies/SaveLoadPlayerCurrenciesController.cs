using Asce.Game.Items;
using Asce.Game.Players;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.SaveLoads
{
    public class SaveLoadPlayerCurrenciesController : SaveLoadController
    {
        protected override void LoadName()
        {
            _name = "Player Currencies";
        }

        public void SaveAllCurrencies()
        {
            foreach (Currency currency in PlayerManager.Instance.Currencies.AllCurrencies)
            {
                SaveCurrency(currency);
            }
        }

        public void SaveCurrency(Currency currency)
        {
            if (currency == null) return;
            if (currency is not ISaveable<CurrencySaveData> saveable) return;
            CurrencySaveData saveData = saveable.Save();

            SaveLoadManager.Instance.SaveIntoFolder("Currencies", currency.Information.Name, saveData);
        }

        public void LoadCurrency(Currency currency)
        {
            if (currency == null) return;
            if (currency is not ILoadable<CurrencySaveData> loadable) return;

            CurrencySaveData saveData = SaveLoadManager.Instance.LoadFromFolder<CurrencySaveData>("Currencies", currency.Information.Name);
            loadable.Load(saveData);
        }

    }
}