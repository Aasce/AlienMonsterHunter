using Asce.Game.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [System.Serializable]
    public class AbilityContainer
    {
        [SerializeField] private string _name;
        [SerializeField] private Cooldown _cooldown = new(10f);

        [Space]
        [SerializeField] private Ability _abilityPrefab;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                if (GameManager.Instance == null)
                {
                    _name = value;
                    _abilityPrefab = null;
                    return;
                }

                Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(value);
                if (abilityPrefab == null || abilityPrefab.Information == null)
                {
                    _name = value;
                    _abilityPrefab = null;
                    return;
                }

                _name = value;
                _abilityPrefab = abilityPrefab;
            }
        }
        public Cooldown Cooldown => _cooldown;
        public Ability AbilityPrefab => _abilityPrefab;

        public bool IsValid => _abilityPrefab != null;

        public AbilityContainer() { }
        public AbilityContainer(string name)
        {
            Name = name;
            if (_abilityPrefab != null)
            {
                _cooldown.BaseTime = _abilityPrefab.Information.Cooldown;
                _cooldown.ToComplete();
            }
        }
    }
}