using Asce.Game.Items;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIItem : UIComponent
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        [Space]
        [SerializeField, Readonly] private Item _item;


        public Item Item
        {
            get => _item;
            set
            {
                if (_item == value) return;
                this.Unregister();
                _item = value;
                this.Register();
            }
        }

        private void OnDestroy()
        {
            this.Unregister();
        }

        protected void Register() 
        {
            if (_item == null) return;
            _icon.sprite = _item.Information.Icon;
            _quantityText.text = _item.Quantity.ToThousandsSeparatedString(sep: ",");

            _item.OnQuantityChanged += Item_OnQuantityChanged;
        }

        protected void Unregister()
        {
            if (_item == null) return;

            _item.OnQuantityChanged -= Item_OnQuantityChanged;
        }

        private void Item_OnQuantityChanged(int newQuantity)
        {
            _quantityText.text = _item.Quantity.ToThousandsSeparatedString(sep: ",");
        }

    }
}
