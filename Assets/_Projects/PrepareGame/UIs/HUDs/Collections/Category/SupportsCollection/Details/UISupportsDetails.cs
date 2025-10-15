using Asce.Game.Supports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UISupportsDetails : UICollectionDetails<Support>
    {
        [Header("Information")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;

        public override void Set(Support support)
        {
            if (Item == support) return;
            this.Unregister();
            Item = support;
            this.Register();
        }


        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            if (_icon != null) _icon.sprite = Item.Information.Icon;
            if (_nameText != null) _nameText.text = Item.Information.Name;
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

        }

    }
}
