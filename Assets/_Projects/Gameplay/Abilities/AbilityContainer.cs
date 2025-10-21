using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [System.Serializable]
    public class AbilityContainer : IIdentifiable, ISaveable<AbilityContainerSaveData>, ILoadable<AbilityContainerSaveData>
    {
        public const string PREFIX_ID = "ability";

        [SerializeField, Readonly] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private Cooldown _cooldown = new(10f);

        [Space]
        [SerializeField] private Ability _abilityPrefab;

        public string Id => _id;
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
                if (abilityPrefab == null)
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
        string IIdentifiable.Id 
        { 
            get => Id; 
            set => _id = value; 
        }

        public AbilityContainer() : this(string.Empty) { }
        public AbilityContainer(string name)
        {
            _id = IdGenerator.NewId(PREFIX_ID);
            Name = name;
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
                name = this.Name,
                cooldown = _cooldown.CurrentTime,
            };
        }

        void ILoadable<AbilityContainerSaveData>.Load(AbilityContainerSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            this.Name = data.name;
            _cooldown.CurrentTime = data.cooldown;
        }
    }
}