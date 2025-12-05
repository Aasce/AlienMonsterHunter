using Asce.Game.Combats;
using Asce.Game.Interactions;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class Entity_SpawnExpOrbOnDeath : GameComponent
    {
        [SerializeField, Readonly] private Entity _entity;
        [SerializeField] private string _expOrbName = "Exp Orb";
        [SerializeField] private int _expAmount = 0;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _entity);
        }

        private void Start()
        {
            if (_entity == null) return;
            _entity.OnDead += Entity_OnDead;
        }

        private void Entity_OnDead(DamageContainer container)
        {
            if (container == null) return;
            if (container.Sender is Enemies.Enemy) return;

            ExpOrb_InteractiveObject expOrb = InteractionController.Instance.Spawn(_expOrbName) as ExpOrb_InteractiveObject;
            if (expOrb == null) return;
            expOrb.transform.position = _entity.transform.position;
            expOrb.ExpAmount = _expAmount;
            expOrb.OnActive();
        }
    }
}
