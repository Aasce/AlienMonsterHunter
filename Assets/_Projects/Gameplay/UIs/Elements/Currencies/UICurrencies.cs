using Asce.Game.Items;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UICurrencies : UIObject
    {
        [SerializeField] private Pool<UICurrency> _pool = new();

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
