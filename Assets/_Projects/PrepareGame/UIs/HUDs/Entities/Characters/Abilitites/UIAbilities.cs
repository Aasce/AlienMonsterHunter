using Asce.Game.Abilities;
using Asce.Game.Managers;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIAbilities : UIObject
    {
        [Header("Reference")]
        [SerializeField] private Pool<UIAbilityInformation> _pools = new();
        [SerializeField] private RectTransform _divider; // divider between passive and active abilities
        [SerializeField] private UIAbilityDetails _details;

        [Header("Config")]
        [SerializeField] private bool _isShowFirstAbilityDetails = false;

        private readonly List<Ability>  _passiveAbilities = new();
        private readonly List<Ability> _activeAbilities = new();

        public List<Ability> Abilities => _passiveAbilities.Concat(_activeAbilities).ToList();

        public void Set(IEnumerable<string> abilitiesName)
        {
            Ability first = null;
            _passiveAbilities.Clear();
            _activeAbilities.Clear();
            foreach (string abilityName in abilitiesName)
            {
                if (string.IsNullOrEmpty(abilityName)) continue;
                Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(abilityName);
                if (abilityPrefab == null) continue;

                if (abilityPrefab.Information.IsPassive) _passiveAbilities.Add(abilityPrefab);
                else _activeAbilities.Add(abilityPrefab);

                if (first == null) first = abilityPrefab;
            }

            this.SetAbilities();
            if (_isShowFirstAbilityDetails) this.ShowDetails(first);
            else this.ShowDetails(null);
        }

        public void ShowDetails(Ability ability)
        {
            if (_details == null) return;

            _details.Set(ability);
            _details.SetVisible(ability != null);
        }

        private void SetAbilities()
        {
            _pools.Clear(onClear: (ability) => ability.Hide());


            this.AbilitiesHandle(_passiveAbilities);
            this.ShowDivider(_passiveAbilities.Count > 0);
            this.AbilitiesHandle(_activeAbilities);
        }

        private void AbilitiesHandle(List<Ability> abilities)
        {
            foreach (Ability ability in abilities)
            {
                UIAbilityInformation uiAbility = _pools.Activate(out bool isCreated);
                if (uiAbility == null) continue;
                if (isCreated) uiAbility.Controller = this;

                uiAbility.Set(ability);
                uiAbility.transform.SetAsLastSibling();
                uiAbility.Show();
            }
        }

        private void ShowDivider(bool isShow)
        {
            if (_divider == null) return;
            _divider.gameObject.SetActive(isShow);
            if (isShow) _divider.SetAsLastSibling();
        }

    }
}
