using Asce.Game.Items;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UICurrency : UIComponent
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        [Space]
        [SerializeField, Readonly] private Currency _currency;


        public Currency Currency
        {
            get => _currency;
            set
            {
                if (_currency == value) return;
                this.Unregister();
                _currency = value;
                this.Register();
            }
        }

        private void OnDestroy()
        {
            this.Unregister();
        }

        protected void Register() 
        {
            if (_currency == null) return;
            _icon.sprite = _currency.Information.Icon;
            _quantityText.text = _currency.Quantity.ToThousandsSeparatedString(sep: ",");

            _currency.OnQuantityChanged += Currency_OnQuantityChanged;
        }

        protected void Unregister()
        {
            if (_currency == null) return;

            _currency.OnQuantityChanged -= Currency_OnQuantityChanged;
        }

        private void Currency_OnQuantityChanged(int newQuantity)
        {
            _quantityText.text = _currency.Quantity.ToThousandsSeparatedString(sep: ",");
        }

    }
}
