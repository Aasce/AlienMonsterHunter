using Asce.Game.Abilities;
using Asce.Game.Entities.Characters;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UICharacterAbilities : UIObject
    {
        [SerializeField] private Pool<UIAbility> _pools = new();
        [SerializeField] private RectTransform _divider; // divider between passive and active abilities


        [Header("Ref")]
        [SerializeField] private CharacterAbilities _characterAbilities;

        public CharacterAbilities CharacterAbilities 
        {
            get => _characterAbilities;
            set
            {
                if (_characterAbilities == value) return;
                this.Unregister();
                _characterAbilities = value;
                this.Register();
            }
        }

        private void Register()
        {
            if (_characterAbilities == null) return;

            this.SetAbilities();
            _characterAbilities.OnInitializeCompleted += CharacterAbilities_OnInitializeCompleted;
        }


        private void Unregister()
        {
            if (_characterAbilities == null) return;
            _characterAbilities.OnInitializeCompleted -= CharacterAbilities_OnInitializeCompleted;
        }

        private void SetAbilities()
        {
            _pools.Clear(onClear: (ability) => ability.Hide());
            this.AbilitiesHandle(_characterAbilities.PassiveAbilities);
            if (_divider != null) _divider.SetAsLastSibling();
            this.AbilitiesHandle(_characterAbilities.ActiveAbilities);
        }

        private void AbilitiesHandle(List<AbilityContainer> abilities) 
        {
            foreach (AbilityContainer container in abilities)
            {
                UIAbility uiAbility = _pools.Activate();
                if (uiAbility == null) continue;

                uiAbility.Container = container;
                uiAbility.transform.SetAsLastSibling();
                uiAbility.Show();
            }
        }


        private void CharacterAbilities_OnInitializeCompleted() => SetAbilities();
    }
}
