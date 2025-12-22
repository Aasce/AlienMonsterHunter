using Asce.Game.Items;
using Asce.Game.Players;
using Asce.Core.UIs;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UIItems : UIComponent
    {
        [SerializeField] private Pool<UIItem> _pool = new();

        [Space]
        [SerializeField] private List<string> _includeCurrencies = new();
        [SerializeField] private List<string> _excludeCurrencies = new();

        private void Start()
        {
            List<Item> filtered = PlayerManager.Instance.Items.AllItems
                .FilterByIncludeExclude(
                    item => item.Information.Name,
                    _includeCurrencies,
                    _excludeCurrencies
                );

            this.Set(filtered);
        }


        public void Set(IEnumerable<Item> items)
        {
            foreach (Item item in items)
            {
                UIItem uiItem = _pool.Activate(out bool isCreated);
                if (uiItem == null) continue;
                uiItem.Item = item;
                uiItem.RectTransform.SetAsLastSibling();
                uiItem.Show();
            }
        }
    }
}
