using Asce.Game.Entities.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Alients
{
    public class UIAlientCollectionItem : UICollectionItem<Enemy>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        public override bool IsUnlocked => true;

        protected override void Register()
        {
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
            this.SetLockedState();
        }
		
    }
}
