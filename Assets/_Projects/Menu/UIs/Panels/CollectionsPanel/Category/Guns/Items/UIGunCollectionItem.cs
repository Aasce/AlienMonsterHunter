using Asce.Game.Guns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.Guns
{
    public class UIGunCollectionItem : UICollectionItem<Gun>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [Header("Purchased")]
        [SerializeField] protected RectTransform _purchasedContent;
        
        [Space]
        [SerializeField] protected RectTransform _buyContent;
        [SerializeField] protected TextMeshProUGUI _buyCostText;
        [SerializeField] protected Button _buyButton;

        protected bool IsPurchased => false;

        protected override void Start()
        {
            base.Start();
            _buyButton.onClick.AddListener(BuyButton_OnClick);
        }

        public override void InternalSet(Gun item)
        {
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
            // _typeText.text = Item.Information.GunType;
            // _difficultSlider.value = Item.Information.Difficulty;
            this.ShowJoinContent();
        }


        protected void ShowJoinContent()
        {
            bool isPurchased = this.IsPurchased;
            _purchasedContent.gameObject.SetActive(isPurchased);
            _buyContent.gameObject.SetActive(!isPurchased);

            if (isPurchased)
            {

            }
            else
            {
                float cost = 100f; // Item.Information.Cost;
                _buyCostText.text = $"${cost}";
            }
        }

        private void BuyButton_OnClick()
        {
            if (IsPurchased) return;
        }

    }
}
