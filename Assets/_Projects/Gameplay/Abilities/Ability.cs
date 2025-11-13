using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public abstract class Ability : GameComponent, IIdentifiable, ISaveable<AbilitySaveData>, ILoadable<AbilitySaveData>
    {
        public const string PREFIX_ID = "ability";

        [SerializeField, Readonly] protected string _id = string.Empty;
        [SerializeField] protected SO_AbilityInformation _information;
        [SerializeField, Readonly] protected Leveling _leveling;
        [SerializeField, Readonly] protected GameObject _owner;
        [SerializeField, Readonly] protected Cooldown _despawnTime = new (10f);

        public string Id => _id;
        public SO_AbilityInformation Information => _information;
        public Leveling Leveling => _leveling;
        public virtual GameObject Owner
        {
            get => _owner;
            set => _owner = value;
        }
        public Cooldown DespawnTime => _despawnTime;

        string IIdentifiable.Id 
        {
            get => Id;
            set => _id = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _leveling);
        }

        protected virtual void Start()
        {
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(this._id)) this._id = IdGenerator.NewId(PREFIX_ID);
            DespawnTime.SetBaseTime(Information.DaspawnTime);
            Leveling.Initialize(Information.Leveling);

            Leveling.OnLevelSetted += Leveling_OnLevelSetted;
            Leveling.OnLevelUp += Leveling_OnLevelUp;
        }

        public virtual void ResetStatus()
        {
            this.DespawnTime.Reset();
        }

        public virtual void OnSpawn()
        {

        }

        public virtual void OnActive() { }

        public virtual void OnDespawn()
        {

        }

        protected virtual void LevelTo(int newLevel)
        {
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("DespawnTime", out LevelModification despawnTimeModification))
            {
                DespawnTime.BaseTime += despawnTimeModification.Value;
            }
        }

        protected virtual void Leveling_OnLevelSetted(int newLevel)
        {
            DespawnTime.SetBaseTime(Information.DaspawnTime);
            for (int i = 1; i <= newLevel; i++)
            {
                this.LevelTo(i);
            }
        }

        protected virtual void Leveling_OnLevelUp(int newLevel) => LevelTo(newLevel);

        AbilitySaveData ISaveable<AbilitySaveData>.Save()
        {
            AbilitySaveData abilityData = new ()
            {
                id = this.Id,
                name = Information.Name,
                ownerId = this.Owner.GetId(),
                baseDespawnTime = this.DespawnTime.BaseTime,
                despawnTime = this.DespawnTime.CurrentTime,
                position = this.transform.position,
                rotation = this.transform.eulerAngles.z,
            };
            this.OnBeforeSave(abilityData);
            return abilityData;
        }

        void ILoadable<AbilitySaveData>.Load(AbilitySaveData data)
        {
            if (data == null) return;
            this._id = data.id;
            this._despawnTime.BaseTime = data.baseDespawnTime;
            this._despawnTime.CurrentTime = data.despawnTime;
            this.transform.position = data.position;
            this.transform.eulerAngles = new Vector3(0f, 0f, data.rotation);
            this._owner = ComponentUtils.FindGameObjectById(data.ownerId);
            StartCoroutine(LoadOwner(data));
            this.OnAfterLoad(data);

            IEnumerator LoadOwner(AbilitySaveData data)
            {
                yield return null;
                this._owner = ComponentUtils.FindGameObjectById(data.ownerId);
            }
        }

        protected virtual void OnBeforeSave(AbilitySaveData data) { }
        protected virtual void OnAfterLoad(AbilitySaveData data) { }
    }
}