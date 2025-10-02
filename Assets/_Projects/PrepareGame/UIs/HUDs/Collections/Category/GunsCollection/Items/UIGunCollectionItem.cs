using Asce.Game.Guns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UIGunCollectionItem : UICollectionItem<Gun>
    {
        [Header("Gun")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private TextMeshProUGUI _purchasedText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;

        protected override void InternalSet(Gun gun)
        {
            if (gun == null || gun.Information == null)
            {
                if (_icon != null) _icon.sprite = null;
                if (_nameText != null) _nameText.text = "Unknown";
                return;
            }

            if (_icon != null) _icon.sprite = gun.Information.Icon;
            if (_nameText != null) _nameText.text = gun.Information.Name;
            this.SetBuyButton();
        }

        private void SetBuyButton()
        {
            bool isBought = false;
            if (isBought)
            {
                if (_buyButton != null) _buyButton.gameObject.SetActive(false);
                if (_priceText != null) _priceText.gameObject.SetActive(false);
                if (_levelText != null)
                {
                    _levelText.gameObject.SetActive(true);
                    _levelText.text = $"lv. NaN";
                }
            }
            else
            {
                if (_levelText != null) _levelText.gameObject.SetActive(false);
                if (_buyButton != null)
                {
                    _buyButton.gameObject.SetActive(true);
                }
                if (_priceText != null)
                {
                    _priceText.gameObject.SetActive(true);
                    _priceText.text = $"${1000}";
                }
            }
        }

    }
}
