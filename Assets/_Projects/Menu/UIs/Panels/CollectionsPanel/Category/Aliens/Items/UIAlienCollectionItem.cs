using Asce.Game.Entities.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UIAlienCollectionItem : UICollectionItem<Enemy>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [SerializeField] private TextMeshProUGUI _speciesText;
        [SerializeField] private Slider _dangerLevelSlider;

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
            _speciesText.text = Item.Information.Species.ToString();
            _dangerLevelSlider.value = Item.Information.DangerLevel;

            this.SetLockedState();
        }
		
    }
}
