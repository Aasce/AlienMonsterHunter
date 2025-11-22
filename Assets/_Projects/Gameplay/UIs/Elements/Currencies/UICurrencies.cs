using Asce.Game.Items;
using Asce.Game.Players;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UICurrencies : UIObject
    {
        [SerializeField] private Pool<UICurrency> _pool = new();

        [Space]
        [SerializeField] private bool _allCurrencies = true;
        [SerializeField] private List<string> _includeCurrencies = new();
        [SerializeField] private List<string> _excludeCurrencies = new();

        private void Start()
        {
            // Get the entire runtime list of currencies
            IEnumerable<Currency> all = PlayerManager.Instance.Currencies.AllCurrencies;
            List<Currency> filtered = new();

            if (_allCurrencies)
            {
                // Include all currencies but exclude some
                foreach (Currency currency in all)
                {
                    string name = currency.Information.Name;
                    if (_excludeCurrencies.Contains(name)) continue;

                    filtered.Add(currency);
                }
            }
            else
            {
                // Include only currencies specified in include list
                foreach (Currency currency in all)
                {
                    string name = currency.Information.Name;
                    if (!_includeCurrencies.Contains(name)) continue;

                    filtered.Add(currency);
                }
            }

            this.Set(filtered);
        }


        public void Set(IEnumerable<Currency> currenies)
        {
            foreach (Currency currency in currenies)
            {
                UICurrency uiCurrency = _pool.Activate(out bool isCreated);
                if (uiCurrency == null) continue;
                uiCurrency.Currency = currency;
                uiCurrency.RectTransform.SetAsLastSibling();
                uiCurrency.Show();
            }
        }
    }
}
