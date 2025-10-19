using Asce.Game.Supports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UISupportCollectionItem : UICollectionItem<Support>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        protected override void InternalSet(Support support)
        {
            if (support == null || support.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = support.Information.Icon;
            _nameText.text = support.Information.Name;
            _levelText.text = "lv. NaN";
        }
    }
}
