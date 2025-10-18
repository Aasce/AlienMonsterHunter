using Asce.Game.Guns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIGunsDetails : UICollectionDetails<Gun>
    {
        [Header("Information")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        [Space]
        [SerializeField] private Button _buyButton;

        [SerializeField] private RectTransform _purchasedContent;
        [SerializeField] private TextMeshProUGUI _level;

        [SerializeField] private UIMagazineGroup _magazineGroup;
        [SerializeField] private UIGunMode _gunMode;


        public override void Set(Gun gun)
        {
            if (Item == gun) return;
            this.Unregister();
            Item = gun;
            this.Register();
        }

        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            if (_nameText != null) _nameText.text = Item.Information.Name;
            if (_icon != null) _icon.sprite = Item.Information.Icon;
            this.SetBuyButton();

            if (_magazineGroup != null) _magazineGroup.Set(Item.Information);
            if (_gunMode != null) _gunMode.Set(null);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;


        }

        private void SetBuyButton()
        {
            bool isBought = false;
            if (isBought)
            {
                if (_buyButton != null) _buyButton.gameObject.SetActive(false);
                if (_purchasedContent != null)
                {
                    _purchasedContent.gameObject.SetActive(true);
                    if (_level != null) _level.text = $"lv. NaN";
                }
            }
            else
            {
                if (_purchasedContent != null) _purchasedContent.gameObject.SetActive(false);
                if (_buyButton != null)
                {
                    _buyButton.gameObject.SetActive(true);
                }
            }
        }

    }
}
