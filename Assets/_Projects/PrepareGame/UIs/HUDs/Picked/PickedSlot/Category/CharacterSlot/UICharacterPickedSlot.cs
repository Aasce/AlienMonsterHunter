using Asce.Game.Entities.Characters;
using Asce.Game.Players;
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

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);

        public override void Initialize()
        {
            base.Initialize();

            this.Set(PickController.Instance.CharacterPrefab);
            PickController.Instance.OnPickCharacter += PickController_OnPickCharacter;
        }

        private void OnDestroy()
        {
            if (PlayerManager.Instance == null) return;
            this.Unregister();
        }

        protected override void Register() 
		{
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _levelText.text = $"lv. {Progress.Level}";

            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        protected override void Unregister()
        {
            base.Unregister();
            if (Item == null || Item.Information == null) return;
            Progress.OnLevelChanged -= Progress_OnLevelChanged;
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

        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }

    }
}
