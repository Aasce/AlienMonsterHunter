using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Game.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.ResultGame.UIs.HUDs
{
    public class UIAwardItem : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        [Header("Runtime")]
        [SerializeField, Readonly] private Award _award;

        public void Set(Award award)
        {
            if (_award == award) return;
            _award = award;
            this.Register();
        }

        private void Register()
        {
            if (_award == null)
            {
                return;
            }

            SO_ItemInformation item = GameManager.Instance.AllItems.Get(_award.ItemName);
            if (item == null)
            {
                return;
            }

            _itemNameText.text = _award.ItemName;
            _icon.sprite = item.Icon;
            _quantityText.text = $"{_award.Quantity:+0.#}";
        }

    }
}
