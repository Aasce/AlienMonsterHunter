using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Maps
{
    public class UIMapCollectionItem : UICollectionItem<Map>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _thumbnail;

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
            _thumbnail.sprite = Item.Information.Thumbnail;
            this.SetLockedState();
        }
		
    }
}
