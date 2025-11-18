using Asce.Game.Guns;
using System;
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

        [Header("Purchase")]
        [SerializeField] private RectTransform _purchasedContent;
        [SerializeField] private TextMeshProUGUI _purchasedText;

        [Space]
        [SerializeField] private RectTransform _buyContent;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Button _buyButton;

        public bool IsPurchased => false;

        protected override void Start()
        {
            base.Start();
            _buyButton.onClick.AddListener(BuyButton_OnClick);
        }

        protected override void Register()
        {
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            this.SetBuyButton();
        }

        private void SetBuyButton()
        {
            bool isPurchased = this.IsPurchased;
            _purchasedContent.gameObject.SetActive(isPurchased);
            _levelText.gameObject.SetActive(isPurchased);
            _buyContent.gameObject.SetActive(!isPurchased);

            if (isPurchased)
            {
                _levelText.text = $"lv. {100}";
            }
            else
            {
                _priceText.text = $"${1000}";
            }
        }

        private void BuyButton_OnClick()
        {
            if (this.IsPurchased) return;


        }

    }
}
