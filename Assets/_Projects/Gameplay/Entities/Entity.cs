using Asce.Managers;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class Entity : GameComponent
    {
        [Header("Entity")]
        [SerializeField] protected SO_EntityInformation _information;
        [SerializeField] protected EntityView _view;
        [SerializeField] protected EntityStats _stats;

        public event Action<float> OnTakeDamage;

        public SO_EntityInformation Information => _information;
        public EntityView View => _view;
        public EntityStats Stats => _stats;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _stats);
            this.LoadComponent(out _view);
        }

        protected virtual void Start()
        {
            if (Information == null) return;
            if (Stats != null) Stats.Initialize(Information.Stats);
        }

        public virtual void ResetStatus()
        {
            Stats.ResetStats();
        }

        public virtual void TakeDamageCallback(float damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}