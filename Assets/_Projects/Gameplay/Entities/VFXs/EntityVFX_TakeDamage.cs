using Asce.Game.VFXs;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    [RequireComponent(typeof(Entity))]
    public class EntityVFX_TakeDamage : GameComponent
    {
        [SerializeField, Readonly] private Entity _entity;

        [Space]
        [SerializeField] private string _takeDamageVFXName = string.Empty;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _entity);
        }

        private void Start()
        {
            if (_entity == null) return;
            _entity.OnAfterTakeDamage += Entity_OnTakeDamage;
        }

        private void Entity_OnTakeDamage(Combats.DamageContainer container)
        {
            VFXController.Instance.Spawn(_takeDamageVFXName, _entity.transform.position);
        }
    }
}