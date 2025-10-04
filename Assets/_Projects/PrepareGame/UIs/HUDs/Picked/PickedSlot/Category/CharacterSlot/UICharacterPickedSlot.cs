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

        protected override void InternalSet(Character item) 
		{
            PickController.Instance.PickCharacter(item);
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
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

    }
}
