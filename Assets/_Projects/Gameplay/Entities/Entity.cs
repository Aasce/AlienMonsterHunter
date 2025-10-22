using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class Entity : GameComponent, IIdentifiable, ITakeDamageable, ITargetable, ISaveable<EntitySaveData>, ILoadable<EntitySaveData>
    {
        public const string PREFIX_ID = "entity";

        [Header("Entity")]
        [SerializeField, Readonly] protected string _id;
        [SerializeField] protected SO_EntityInformation _information;
        [SerializeField, Readonly] protected EntityView _view;
        [SerializeField, Readonly] protected EntityStats _stats;
        [SerializeField, Readonly] protected EntityEffects _effects;

        protected bool _isDeath = false;

        public event Action<float> OnTakeDamage;
        public event Action OnDead;

        public string Id => _id;
        public SO_EntityInformation Information => _information;
        public EntityView View => _view;
        public EntityStats Stats => _stats;
        public EntityEffects Effects => _effects;

        public bool IsDeath => _isDeath;

        string IIdentifiable.Id 
        { 
            get => Id;
            set => _id = value;
        }
        ResourceStat ITakeDamageable.Health => Stats.Health;
        Stat ITakeDamageable.Armor => Stats.Armor;
        bool ITargetable.IsTargetable => true;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _view);
            this.LoadComponent(out _stats);
            this.LoadComponent(out _effects);
        }

        protected virtual void Start() { }

        public virtual void Initialize()
        {
            Stats.Initialize(Information.Stats);
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
        }

        public virtual void ResetStatus()
        {
            Stats.ResetStats();
            Effects.Clear();
            _isDeath = false;
        }

        protected virtual void Dead()
        {
            if (_isDeath) return;
            _isDeath = true;
            Effects.Clear();
            OnDead?.Invoke();
        }


        void ITakeDamageable.TakeDamageCallback(float damage)
        {
            OnTakeDamage?.Invoke(damage);
            if (Stats == null) return;
            if (Stats.Health.CurrentValue <= 0f) this.Dead();
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

            if (Stats is ISaveable<StatsSaveData> statsSaveable)
            {
                saveData.stats = statsSaveable.Save();
            }
            return saveData;
        }

        void ILoadable<EntitySaveData>.Load(EntitySaveData data)
        {
            if (data == null) return;
            this._id = data.id;
            this.transform.position = data.position;
            this.transform.eulerAngles = new Vector3(0f, 0f, data.rotation);

            if (Stats is ILoadable<StatsSaveData> statsLoadable)
            {
                statsLoadable.Load(data.stats);
            }
        }
    }
}