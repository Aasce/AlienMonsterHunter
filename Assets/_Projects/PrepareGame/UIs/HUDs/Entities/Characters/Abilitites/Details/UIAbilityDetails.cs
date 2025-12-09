using Asce.Game.Abilities;
using Asce.Core.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Asce.Core.Attributes;
using Asce.Game;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs
{
    public class UIAbilityDetails : UIComponent
    {
        [Header("References")]
        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _activeTypeText;

        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Transform _descriptionDivider;
        [SerializeField] private TextMeshProUGUI _sideNotesText;

        [Header("Runtime")]
        [SerializeField, Readonly] private Ability _ability;

        private void Start()
        {
            _hideButton.onClick.AddListener(HideButton_OnClick);
            _upgradeButton.onClick.AddListener(UpgrandButton_OnClick);
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

            _nameText.text = _ability.Information.Name;
            _levelText.text = "lv.NaN";
            _cooldownText.text = $"CD: {_ability.Information.Cooldown:#.#}s";
            _activeTypeText.text = _ability.Information.IsActive ? "Active" : "Passive";
            this.SetDescription();
        }

        private void Unregister()
        {
            if (_ability == null) return;
        }

        private void SetDescription()
        {
            Dictionary<string, string> values = new()
            {
                { "DespawnTime", $"{_ability.Information.DespawnTime:0.#}" },
                { "UseRangeRadius", $"{_ability.Information.UseRangeRadius:0.#}" },
                { "Cooldown", $"{_ability.Information.Cooldown:0.#}" },
                { "ReactiveCooldown", $"{_ability.Information.ReactiveCooldown:0.#}" }
            };
            foreach (var custom in _ability.Information.Customs)
            {
                values.Add(custom.Name, $"{custom.Value:0.#}");
            }
            if (_ability is IControlMachineAbility controlMachineAbility)
            {
                values.Add("Health", $"{controlMachineAbility.Machine.Information.Stats.MaxHealth:0.#}");
                values.Add("Armor", $"{controlMachineAbility.Machine.Information.Stats.Armor:0.#}");
                values.Add("Speed", $"{controlMachineAbility.Machine.Information.Stats.Speed:0.#}");
                foreach (var custom in controlMachineAbility.Machine.Information.Stats.CustomStats)
                {
                    values.Add(custom.Name, $"{custom.Value:0.#}");
                }
            }

            string description = _ability.Information.Description.GetDescription(values);
            _descriptionText.text = description;

            if (string.IsNullOrEmpty(_ability.Information.SideNote))
            {
                _descriptionDivider.gameObject.SetActive(false);
                _sideNotesText.text = string.Empty;
                return;
            }

            _descriptionDivider.gameObject.SetActive(true);

            string sideNotes = _ability.Information.SideNote.GetDescription(values);
            _sideNotesText.text = sideNotes;
        }

        private void HideButton_OnClick() => this.Hide();
        private void UpgrandButton_OnClick()
        {
            Debug.Log($"Ability {_ability.Information.Name} upgrade");
        }

    }
}
