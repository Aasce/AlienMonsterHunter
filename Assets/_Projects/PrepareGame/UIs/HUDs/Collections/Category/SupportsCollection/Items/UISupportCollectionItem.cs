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
                if (_icon != null) _icon.sprite = null;
                if (_nameText != null) _nameText.text = "Unknown";
                if (_levelText != null) _levelText.text = "lv. NaN";
                return;
            }

            if (_icon != null) _icon.sprite = support.Information.Icon;
            if (_nameText != null) _nameText.text = support.Information.Name;
            if (_levelText != null) _levelText.text = "lv. NaN";
        }
    }
}
