using Asce.Game.Abilities;
using Asce.Game.Entities.Characters;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.UIs.HUDs
{
    public class UICharacterAbilities : UIObject
    {
        [Header("References")]
        [SerializeField] private Pool<UIAbility> _pools = new();
        [SerializeField] private RectTransform _divider; // divider between passive and active abilities

        [Space]
        [SerializeField] private CharacterAbilities _characterAbilities;
        [SerializeField, Readonly] private List<KeyCode> _useKeys = new();

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

        public void SetUseKeys(List<KeyCode> keys)
        {
            if (keys == null) return;
            _useKeys.Clear();
            _useKeys.AddRange(keys);
        }


        private void Register()
        {
            if (_characterAbilities == null) return;

            this.SetAbilities();
            _characterAbilities.OnAbilityChanged += CharacterAbilities_OnInitializeCompleted;
        }


        private void Unregister()
        {
            if (_characterAbilities == null) return;
            _characterAbilities.OnAbilityChanged -= CharacterAbilities_OnInitializeCompleted;
        }

        private void SetAbilities()
        {
            _pools.Clear(onClear: (ability) => ability.Hide());
            this.AbilitiesHandle(_characterAbilities.PassiveAbilities);
            if (_divider != null) _divider.SetAsLastSibling();
            this.AbilitiesHandle(_characterAbilities.ActiveAbilities, isShowKey: true);
        }

        private void AbilitiesHandle(List<AbilityContainer> abilities, bool isShowKey = false) 
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                AbilityContainer container = abilities[i];
                UIAbility uiAbility = _pools.Activate();
                if (uiAbility == null) continue;

                uiAbility.SetKey(isShowKey ? this.GetKey(i) : KeyCode.None);
                uiAbility.Container = container;
                uiAbility.transform.SetAsLastSibling();
                uiAbility.Show();
            }
        }

        private KeyCode GetKey(int index)
        {
            if (index < 0 || index >= _useKeys.Count) return KeyCode.None;
            return _useKeys[index];
        }

        private void CharacterAbilities_OnInitializeCompleted() => SetAbilities();
    }
}
