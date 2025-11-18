using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.Game.UIs;
using Asce.SaveLoads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [Space]
        [SerializeField] private UITintColor _tintColor;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _roleValueText;
        [SerializeField] private Slider _difficultySlider;

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);

        protected void OnDestroy()
        {
            if (PlayerManager.Instance == null) return;
            this.Unregister();
        }


        protected override void Register()
        {
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _roleValueText.text = Item.Information.Role.ToString();
            _difficultySlider.value = Item.Information.Difficulty;
            this.SetLockedState();

            Progress.OnUnlocked += SetLockedState;
        }

        protected override void Unregister()
        {
            base.Unregister(); 
            if (Item == null || Item.Information == null) return;

            Progress.OnUnlocked -= SetLockedState;
        }

        protected virtual void SetLockedState()
        {
            CharacterProgress progress = Progress;
            bool isUnlocked = progress != null && progress.IsUnlocked;
            if (isUnlocked)
            {
                _tintColor.TintColor = Color.white;
                _levelText.gameObject.SetActive(true);
                _levelText.text = $"lv. {progress.Level}";
            }
            else
            {
                _tintColor.TintColor = Color.gray;
                _levelText.gameObject.SetActive(false);
            }

        }
    }
}
