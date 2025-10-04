using Asce.Game.Entities.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _roleValueText;
        [SerializeField] private Slider _difficultSlider;

        protected override void InternalSet(Character character)
        {
            if (character == null || character.Information == null)
            {
                if (_icon != null) _icon.sprite = null;
                if (_nameText != null) _nameText.text = "Unknown";
                if (_levelText != null) _levelText.text = "lv. NaN";
                if (_roleValueText != null) _roleValueText.text = "Unknown";
                if (_difficultSlider != null) _difficultSlider.value = 0;
                return;
            }

            if (_icon != null) _icon.sprite = character.Information.Icon;
            if (_nameText != null) _nameText.text = character.Information.Name; 
            if (_levelText != null) _levelText.text = "lv. NaN";
            if (_roleValueText != null) _roleValueText.text = "Unknown";
            if (_difficultSlider != null) _difficultSlider.value = 0;
        }
    }
}
