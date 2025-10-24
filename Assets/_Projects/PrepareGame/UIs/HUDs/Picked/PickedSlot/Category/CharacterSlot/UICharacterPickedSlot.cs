using Asce.Game.Entities.Characters;
using Asce.PrepareGame.Picks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UICharacterPickedSlot : UIPickedSlot<Character>
    {
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public override void Initialize()
        {
            base.Initialize();

            this.Set(PickController.Instance.CharacterPrefab);
            PickController.Instance.OnPickCharacter += PickController_OnPickCharacter;
        }

        protected override void InternalSet(Character item) 
		{
		    if (item == null || item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            if (_icon != null) _icon.sprite = item.Information.Icon;
            if (_nameText != null) _nameText.text = item.Information.Name;
            if (_levelText != null) _levelText.text = $"lv.NaN";
        }

        protected override void DiscardButton_OnClick()
        {
            base.DiscardButton_OnClick();
            PickController.Instance.PickCharacter(null);
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        private void PickController_OnPickCharacter(Character character)
        {
            this.Set(character);
        }

    }
}
