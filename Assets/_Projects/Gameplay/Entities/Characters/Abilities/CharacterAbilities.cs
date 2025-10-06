using Asce.Game.Abilities;
using Asce.Managers;
using Asce.Managers.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterAbilities : GameComponent
    {
        [SerializeField, Readonly] private Character _owner;

        [Space]
        [SerializeField, Readonly] private List<AbilityContainer> _abilities = new();
        private readonly List<AbilityContainer> _passiveAbilities = new();
        private readonly List<AbilityContainer> _activeAbilities = new();

        public event Action OnInitializeCompleted;

        public List<AbilityContainer> Abilities => _abilities;
        public List<AbilityContainer> PassiveAbilities => _passiveAbilities;
        public List<AbilityContainer> ActiveAbilities => _activeAbilities;

        private void Update()
        {
            foreach (AbilityContainer ability in _abilities)
            {
                if (ability == null) continue;
                ability.Cooldown.Update(Time.deltaTime);
            }
        }


        public void Initialize(Character owner)
        {
            if (owner == null) return;
            if (owner.Information == null) return;
            _owner = owner;

            _abilities.Clear();
            _passiveAbilities.Clear();
            _activeAbilities.Clear();
            foreach (string abilityName in _owner.Information.AbilitiesName)
            {
                if (string.IsNullOrEmpty(abilityName)) continue;
                AbilityContainer abilityContainer = new(abilityName);
                if (!abilityContainer.IsValid)
                {
                    Debug.LogError($"Abilitiy with name {abilityName} not exist.", _owner.Information);
                    continue;
                }

                _abilities.Add(abilityContainer);
                if (abilityContainer.AbilityPrefab.Information.IsPassive) _passiveAbilities.Add(abilityContainer);
                else _activeAbilities.Add(abilityContainer);
            }

            this.OnInitializeCompleted?.Invoke();
        }

        public void Use(int index, Vector2 position)
        {
            if (_abilities.Count == 0) return;
            if (index < 0 || index >= _abilities.Count) return;

            AbilityContainer container = _abilities[index];
            if (container == null) return;
            if (!container.Cooldown.IsComplete) return;

            string abilityName = container.Name;
            CharacterAbility ability = AbilityController.Instance.Spawn(abilityName, _owner.gameObject) as CharacterAbility;
            if (ability == null) return;

            ability.SetPosition(position);
            ability.gameObject.SetActive(true);
            container.Cooldown.Reset();
        }
    }
}