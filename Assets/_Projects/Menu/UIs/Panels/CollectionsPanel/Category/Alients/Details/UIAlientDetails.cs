using Asce.Game;
using Asce.Game.Entities.Enemies;
using Asce.Game.UIs.Elements;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIAlientDetails : UICollectionDetails<Enemy>
    {
        [Header("Information")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _speciesText;
        [SerializeField] private Slider _dangerLevelSlider;

        [Header("Locked")]
        [SerializeField] private RectTransform _lockContent;
        [SerializeField] private RectTransform _unlockContent;

        [Header("Description")]
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Transform _descriptionDivider;
        [SerializeField] private TextMeshProUGUI _sideNotesText;

        [Header("Stats")]
        [SerializeField] private UIStatsGroup _statGroup;



        public override void Set(Enemy enemy)
        {
            if (Item == enemy) return;
            this.Unregister();
            Item = enemy;
            this.Register();
        }

        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            this.SetLockedState();
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _speciesText.text = Item.Information.Species.ToString();
            _dangerLevelSlider.value = Item.Information.DangerLevel;

            this.SetDescription();
            _statGroup.Set(Item.Information);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;



        }

        private void SetLockedState()
        {
            bool isUnlocked = true;
            _unlockContent.gameObject.SetActive(isUnlocked);
            _lockContent.gameObject.SetActive(!isUnlocked);

            if (isUnlocked)
            {

            }
            else
            {

            }
        }

        private void SetDescription()
        {
            Dictionary<string, string> values = DescriptionUtils.GetEntityDescriptionKeys(Item);
            string description = Item.Information.Description.GetDescription(values);
            _descriptionText.text = description;

            if (string.IsNullOrEmpty(Item.Information.Description.SideNote))
            {
                _descriptionDivider.gameObject.SetActive(false);
                _sideNotesText.text = string.Empty;
                return;
            }

            _descriptionDivider.gameObject.SetActive(true);

            string sideNotes = Item.Information.Description.GetSideNote(values);
            _sideNotesText.text = sideNotes;
        }
    }
}
