using Asce.Game.Abilities;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterAbilities : GameComponent, ISaveable<CharacterAbilitiesSaveData>, ILoadable<CharacterAbilitiesSaveData>
    {
        [SerializeField, Readonly] private Character _owner;

        [Space]
        [SerializeField, Readonly] private List<AbilityContainer> _abilities = new();
        private readonly List<AbilityContainer> _passiveAbilities = new();
        private readonly List<AbilityContainer> _activeAbilities = new();

        public event Action OnAbilityChanged;

        public List<AbilityContainer> Abilities => _abilities;
        public List<AbilityContainer> PassiveAbilities => _passiveAbilities;
        public List<AbilityContainer> ActiveAbilities => _activeAbilities;

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

            this.OnAbilityChanged?.Invoke();
        }

        public void ResetStatus()
        {
            foreach (AbilityContainer ability in _abilities)
            {
                if (ability == null) continue;

            }
        }

        private void Update()
        {
            foreach (AbilityContainer container in _abilities)
            {
                if (container == null) continue;
                container.Cooldown.Update(Time.deltaTime);
                if (container.AbilityInstance != null)
                {
                    if (!container.AbilityInstance.gameObject.activeInHierarchy)
                    {
                        container.AbilityInstance = null;
                        container.Cooldown.SetBaseTime(container.Information.Cooldown);
                    }
                }
            }
        }

        public void Use(int index, Vector2 position)
        {
            if (_abilities.Count == 0) return;
            if (index < 0 || index >= _abilities.Count) return;

            AbilityContainer container = _abilities[index];
            if (container == null) return;
            if (!container.Cooldown.IsComplete) return;

            if (container.Information.IsReactive && container.IsValidInstance)
            {
                this.Reactive(container, position);
                return;
            }

            this.SpawnAbility(container, position);
        }

        private void SpawnAbility(AbilityContainer container, Vector2 position)
        {
            string abilityName = container.Name;
            CharacterAbility ability = AbilityController.Instance.Spawn(abilityName, _owner.gameObject) as CharacterAbility;
            if (ability == null) return;

            ability.Leveling.SetLevel(container.Level);
            ability.SetPosition(position);
            ability.gameObject.SetActive(true);
            ability.OnActive();
            if (!container.Information.IsReactive)
            {
                container.Cooldown.SetBaseTime(container.Information.Cooldown);
                return;
            } 
            
            if (container.AbilityInstance == null || !container.AbilityInstance.gameObject.activeInHierarchy)
            {
                container.AbilityInstance = ability;
                container.Cooldown.SetBaseTime(container.Information.ReactiveCooldown);
            }
        }

        private void Reactive(AbilityContainer container, Vector2 position)
        {
            container.AbilityInstance.Reactive(position);
            container.Cooldown.SetBaseTime(container.Information.ReactiveCooldown);
        }

        CharacterAbilitiesSaveData ISaveable<CharacterAbilitiesSaveData>.Save()
        {
            CharacterAbilitiesSaveData abilitiesData = new();
            foreach (AbilityContainer ability in _abilities)
            {
                AbilityContainerSaveData abilityData = (ability as ISaveable<AbilityContainerSaveData>).Save();
                abilitiesData.abilityContainers.Add(abilityData);
            }
            return abilitiesData;
        }

        void ILoadable<CharacterAbilitiesSaveData>.Load(CharacterAbilitiesSaveData data)
        {
            if (data == null) return;

            _abilities.Clear();
            _passiveAbilities.Clear();
            _activeAbilities.Clear();
            foreach (AbilityContainerSaveData abilityData in data.abilityContainers)
            {
                AbilityContainer abilityContainer = new();
                (abilityContainer as ILoadable<AbilityContainerSaveData>).Load(abilityData);

                _abilities.Add(abilityContainer);
                if (abilityContainer.AbilityPrefab.Information.IsPassive) _passiveAbilities.Add(abilityContainer);
                else _activeAbilities.Add(abilityContainer);
            }

            OnAbilityChanged?.Invoke();
        }
    }
}