using Asce.Game.Combats;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class Entity : GameComponent, IIdentifiable, ITakeDamageable, ISendDamageable, ITargetable, ISaveable<EntitySaveData>, ILoadable<EntitySaveData>
    {
        public const string PREFIX_ID = "entity";

        [Header("Entity")]
        [SerializeField, Readonly] protected string _id;
        [SerializeField] protected SO_EntityInformation _information;
        [SerializeField, Readonly] protected Leveling _leveling;
        [SerializeField, Readonly] protected EntityView _view;
        [SerializeField, Readonly] protected EntityStats _stats;
        [SerializeField, Readonly] protected EntityEffects _effects;

        protected bool _isDeath = false;
        protected bool _isTargetable = true;

        public event Action<DamageContainer> OnBeforeTakeDamage;
        public event Action<DamageContainer> OnAfterTakeDamage;
        public event Action<DamageContainer> OnBeforeSendDamage;
        public event Action<DamageContainer> OnAfterSendDamage;
        public event Action<DamageContainer> OnDead;
        public event Action<DamageContainer> OnKill;

        public string Id => _id;
        public SO_EntityInformation Information => _information;
        public Leveling Leveling => _leveling;
        public EntityView View => _view;
        public EntityStats Stats => _stats;
        public EntityEffects Effects => _effects;

        public bool IsDeath => _isDeath;
        public bool IsTargetable
        {
            get => _isTargetable;
            set => _isTargetable = value;
        }

        string IIdentifiable.Id 
        { 
            get => Id;
            set => _id = value;
        }

        ResourceStat ITakeDamageable.Health => Stats.Health;
        Stat ITakeDamageable.Armor => Stats.Armor;
       

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _leveling);
            this.LoadComponent(out _view);
            this.LoadComponent(out _stats);
            if (this.LoadComponent(out _effects))
            {
                _effects.Entity = this;
            }
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            Stats.Initialize(Information.Stats);
            Effects.Initialize();
            Leveling.Initialize(Information.Leveling);
            Leveling.OnLevelSetted += Leveling_OnLevelSetted;
            Leveling.OnLevelUp += Leveling_OnLevelUp;
        }

        public virtual void ResetStatus()
        {
            Stats.ResetStats();
            Effects.Clear();
            _isDeath = false;
        }

        protected virtual void LevelTo(int newLevel)
        {
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("Health", out LevelModification healthModification))
            {
                Stats.Health.Add(healthModification.Value, healthModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("Armor", out LevelModification armorModification))
            {
                Stats.Armor.Add(armorModification.Value, armorModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("Speed", out LevelModification speedModification))
            {
                Stats.Speed.Add(speedModification.Value, speedModification.Type.ToStatType(), StatSourceType.Levelup);
            }
        }

        protected virtual void Leveling_OnLevelSetted(int newLevel)
        {
            Stats.Health.Clear(StatSourceType.Levelup);
            Stats.Armor.Clear(StatSourceType.Levelup);
            Stats.Speed.Clear(StatSourceType.Levelup);

            for (int i = 1; i <= newLevel; i++)
            {
                this.LevelTo(i);
            }
        }

        protected virtual void Leveling_OnLevelUp(int newLevel) => LevelTo(newLevel);

        void ISendDamageable.BeforeSendDamageCallback(DamageContainer container)
        {
            OnBeforeSendDamage?.Invoke(container);
        }

        void ISendDamageable.AfterSendDamageCallback(DamageContainer container)
        {
            OnAfterSendDamage?.Invoke(container);
        }
        void ISendDamageable.KillCallback(DamageContainer container)
        {
            OnKill?.Invoke(container);
        }

        void ITakeDamageable.BeforeTakeDamageCallback(DamageContainer container)
        {
            OnBeforeTakeDamage?.Invoke(container);
        }

        void ITakeDamageable.AfterTakeDamageCallback(DamageContainer container)
        {
            OnAfterTakeDamage?.Invoke(container);
        }

        void ITakeDamageable.DeadCallback(DamageContainer container)
        {
            if (_isDeath) return;
            _isDeath = true;
            Effects.Clear();
            OnDead?.Invoke(container);
        }

        EntitySaveData ISaveable<EntitySaveData>.Save()
        {
            EntitySaveData saveData = new()
            {
                id = this.Id,
                name = this.Information.Name,
                position = this.transform.position,
                rotation = this.transform.eulerAngles.z
            };

            if (Leveling is ISaveable<LevelingSaveData> levelingSaveable)
            {
                saveData.leveling = levelingSaveable.Save();
            }

            if (Stats is ISaveable<StatsSaveData> statsSaveable)
            {
                saveData.stats = statsSaveable.Save();
            }

            if (Effects is ISaveable<EffectsSaveData> effectSaveable)
            {
                saveData.effects = effectSaveable.Save();
            }

            return saveData;
        }

        void ILoadable<EntitySaveData>.Load(EntitySaveData data)
        {
            if (data == null) return;
            this._id = data.id;
            this.transform.position = data.position;
            this.transform.eulerAngles = new Vector3(0f, 0f, data.rotation);

            if (Leveling is ILoadable<LevelingSaveData> levelingLoadable)
            {
                levelingLoadable.Load(data.leveling);
            }

            if (Stats is ILoadable<StatsSaveData> statsLoadable)
            {
                statsLoadable.Load(data.stats);
            }

            if (Effects is ILoadable<EffectsSaveData> effectLoadable)
            {
                effectLoadable.Load(data.effects);
            }
        }

    }
}