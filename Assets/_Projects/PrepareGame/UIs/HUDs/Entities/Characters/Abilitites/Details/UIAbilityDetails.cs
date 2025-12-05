using Asce.Game.Abilities;
using Asce.Core.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIAbilityDetails : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _activeTypeText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _upgradeButton;

        private Ability _ability;

        private void Start()
        {
            if (_hideButton != null) _hideButton.onClick.AddListener(HideButton_OnClick);
        }


        public void Set(Ability ability)
        {
            if (_ability == ability) return;
            this.Unregister();
            _ability = ability;
            this.Register();
        }

        private void Register()
        {
            if (_ability == null) return;

            if (_nameText != null) _nameText.text = _ability.Information.Name;
            if (_levelText != null) _levelText.text = "lv.NaN";
            if (_cooldownText != null) _cooldownText.text = $"CD: {_ability.Information.Cooldown:#.#}s";
            if (_activeTypeText != null) _activeTypeText.text = _ability.Information.IsActive ? "Active" : "Passive";
            if (_descriptionText != null) _descriptionText.text = _ability.Information.Description;
            if (_upgradeButton != null) _upgradeButton.onClick.AddListener(UpgrandButton_OnClick);
        }

        private void Unregister()
        {
            if (_ability == null) return;
            if (_upgradeButton != null) _upgradeButton.onClick.RemoveListener(UpgrandButton_OnClick);
        }

        private void HideButton_OnClick() => this.Hide();
        private void UpgrandButton_OnClick()
        {
            Debug.Log($"Ability {_ability.Information.Name} upgrade");
        }

    }
}
