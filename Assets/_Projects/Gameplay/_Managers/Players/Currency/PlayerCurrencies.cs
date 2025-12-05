using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Core;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerCurrencies : GameComponent
    {
        [SerializeField] protected List<Currency> _curencies = new();
        protected Dictionary<string, Currency> _curenciesDictionary;
        protected ReadOnlyCollection<Currency> _curenciesReadonly;

        public ReadOnlyCollection<Currency> AllCurrencies
        {
            get
            {
                if (_curenciesReadonly == null) this.Initialize();
                return _curenciesReadonly;
            }
        }
        public SaveLoadPlayerCurrenciesController SaveLoadCurrenciesController => SaveLoadManager.Instance.GetController("Player Currencies") as SaveLoadPlayerCurrenciesController;

        public virtual void Initialize()
        {
            SaveLoadPlayerCurrenciesController playerCurrenciesController = SaveLoadManager.Instance.GetController("Player Currencies") as SaveLoadPlayerCurrenciesController;
            if (playerCurrenciesController == null) return;

            foreach (SO_ItemInformation information in GameManager.Instance.AllItems.Currencies)
            {
                Currency currency = new(information);
                playerCurrenciesController.LoadCurrency(currency);
                _curencies.Add(currency);
            }

            _curenciesDictionary = new Dictionary<string, Currency>();
            foreach (Currency currency in _curencies)
            {
                _curenciesDictionary[currency.Information.Name] = currency;
            }

            _curenciesReadonly = _curencies.AsReadOnly();
        }

        public Currency Get(string name)
        {
            if (_curenciesDictionary == null) this.Initialize();
            if (_curenciesDictionary.TryGetValue(name, out Currency currency))
            {
                return currency;
            }
            return null;
        }

        public void Add(string name, int amount)
        {
            if (amount <= 0) return;
            Currency currency = this.Get(name);
            if (currency == null) return;

            currency.Quantity += amount;
            SaveLoadCurrenciesController.SaveCurrency(currency);
        }

        public bool TrySpend(string name, int amount)
        {
            if (amount <= 0) return true;
            Currency currency = this.Get(name);
            if (currency == null) return false;

            if (currency.Quantity < amount) return false;
            currency.Quantity -= amount;

            SaveLoadCurrenciesController.SaveCurrency(currency);
            return true;
        }

        public bool CanSpend(string name, int amount)
        {
            if (amount <= 0) return true;
            Currency currency = this.Get(name);
            if (currency == null) return false;
            return currency.Quantity >= amount;
        }


        public void SaveAll() 
        {
            if (SaveLoadCurrenciesController == null) return;
            SaveLoadCurrenciesController.SaveAllCurrencies();
        }
    }
}
