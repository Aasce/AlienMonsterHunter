using Asce.Game.Abilities;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterAbilities : GameComponent
    {
        [SerializeField, Readonly] private Character _owner;

        [Space]
        [SerializeField, Readonly] private List<string> _abilities = new();


        public List<string> Abilities => _abilities;


        public void Initialize(Character owner)
        {
            if (owner == null) return;
            if (owner.Information == null) return;
            _owner = owner;

            _abilities.Clear();
            foreach (string abilityName in _owner.Information.AbilitiesName)
            {
                if (string.IsNullOrEmpty(abilityName)) continue;
                Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(abilityName);
                if (abilityPrefab == null)
                {
                    Debug.LogError($"Abilitiy with name {abilityName} not exist.", _owner.Information);
                    continue;
                }

                _abilities.Add(abilityName);
            }
        }

        public void Use(int index, Vector2 position)
        {
            if (_abilities.Count == 0) return;
            if (index < 0 || index >= _abilities.Count) return;

            string abilityName = _abilities[index];
            CharacterAbility ability = AbilityController.Instance.Spawn(abilityName, _owner.gameObject) as CharacterAbility;
            if (ability == null) return;

            ability.SetPosition(position);
        }
    }
}