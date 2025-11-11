using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [System.Serializable]
    public class AbilityContainer : IIdentifiable, ISaveable<AbilityContainerSaveData>, ILoadable<AbilityContainerSaveData>
    {
        public const string PREFIX_ID = "ability_container";

        [SerializeField, Readonly] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private int _level = 0;
        [SerializeField] private Cooldown _cooldown = new(10f);

        [Space]
        [SerializeField] private Ability _abilityPrefab;

        public string Id => _id;

        public string Name
        {
            get => _name;
            protected set
            {
                if (_name == value) return;
                _name = value;
                this.UpdateAbilityReference();
            }
        }
        public int Level
        {
            get => _level;
            set => _level = value;
        }
        public Cooldown Cooldown => _cooldown;
        public Ability AbilityPrefab => _abilityPrefab;
        public bool IsValid => _abilityPrefab != null;

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
        /// Updates ability name and prefab reference based on the given name.<br/>
        /// If GameManager or the ability is missing, prefab will be set to null.
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

                _level = 0;
            }
        }

        AbilityContainerSaveData ISaveable<AbilityContainerSaveData>.Save()
        {
            return new AbilityContainerSaveData()
            {
                id = _id,
                name = _name,
                cooldown = _cooldown.CurrentTime,
            };
        }

        void ILoadable<AbilityContainerSaveData>.Load(AbilityContainerSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            _name = data.name;
            this.UpdateAbilityReference();
            _cooldown.CurrentTime = data.cooldown;
        }
    }
}
