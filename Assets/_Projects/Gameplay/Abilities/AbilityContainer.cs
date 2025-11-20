using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [System.Serializable]
    public class AbilityContainer : IIdentifiable, ISaveable<AbilityContainerSaveData>, ILoadable<AbilityContainerSaveData>
    {
        public const string PREFIX_ID = "ability_container";

        [SerializeField, Readonly] private string _id;
        [SerializeField] private string _name;

        [Header("Runtime")]
        [SerializeField, Readonly] private Ability _abilityPrefab;
        [SerializeField, Readonly] private Ability _abilityInstance;
        [SerializeField, Readonly] private Cooldown _cooldown = new(10f);
        [SerializeField] private int _level = 0;

        public event Action<int> OnLevelChanged;

        public string Id => _id;
        public string Name => _name;
        
        public Ability AbilityPrefab => _abilityPrefab;
        public SO_AbilityInformation Information => _abilityPrefab != null ? _abilityPrefab.Information : null;
        public Ability AbilityInstance
        {
            get => _abilityInstance;
            set => _abilityInstance = value;
        }
        public Cooldown Cooldown => _cooldown;
        public int Level
        {
            get => _level;
            set
            {
                int newLevel = Mathf.Clamp(value, 0, MaxLevel);
                if (_level == newLevel) return;
                _level = newLevel;
                OnLevelChanged?.Invoke(_level);
            }
        }
        public int MaxLevel => AbilityPrefab != null ? AbilityPrefab.Information.Leveling.MaxLevel : 0;
        public bool IsValid => Information != null;
        public bool IsValidInstance => AbilityInstance != null && AbilityInstance.gameObject.activeInHierarchy;

        string IIdentifiable.Id
        {
            get => Id;
            set => _id = value;
        }

        public AbilityContainer() : this(string.Empty) { }
        public AbilityContainer(string name)
        {
            _id = IdGenerator.NewId(PREFIX_ID);
            _name = name;
            this.UpdateAbilityReference();
        }

        /// <summary>
        ///     Updates ability name and prefab reference based on the given name.<br/>
        ///     If GameManager or the ability is missing, prefab will be set to null.
        /// </summary>
        private void UpdateAbilityReference()
        {
            if (!GameManager.HasInstance)
            {
                _abilityPrefab = null;
                return;
            }

            Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(_name);
            _abilityPrefab = abilityPrefab;
            if (_abilityPrefab != null)
            {
                _cooldown.BaseTime = _abilityPrefab.Information.Cooldown;
                _cooldown.ToComplete();
            }
        }

        AbilityContainerSaveData ISaveable<AbilityContainerSaveData>.Save()
        {
            return new AbilityContainerSaveData()
            {
                id = _id,
                name = _name,
                cooldown = _cooldown.CurrentTime,
                abilityInstanceId = _abilityInstance != null ? _abilityInstance.Id : string.Empty
            };
        }

        void ILoadable<AbilityContainerSaveData>.Load(AbilityContainerSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            _name = data.name;
            this.UpdateAbilityReference();
            _cooldown.CurrentTime = data.cooldown;
            _abilityInstance = ComponentUtils.FindComponentById<Ability>(data.abilityInstanceId);
        }
    }
}
