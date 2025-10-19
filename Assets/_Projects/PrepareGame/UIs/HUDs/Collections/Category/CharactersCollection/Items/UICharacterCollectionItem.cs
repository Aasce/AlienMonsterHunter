using Asce.Game.Entities.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _roleValueText;
        [SerializeField] private Slider _difficultySlider;

        protected override void InternalSet(Character character)
        {
            if (character == null || character.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = character.Information.Icon;
            _nameText.text = character.Information.Name; 
            _levelText.text = $"lv. {100}";
            _roleValueText.text = Item.Information.Role.ToString();
            _difficultySlider.value = Item.Information.Difficulty;
        }
    }
}
