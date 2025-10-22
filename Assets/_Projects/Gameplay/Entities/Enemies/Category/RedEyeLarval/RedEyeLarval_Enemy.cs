using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class RedEyeLarval_Enemy : Enemy
    {
        public override void Initialize()
        {
            base.Initialize();
            Agent.stoppingDistance = Stats.AttackRange.FinalValue * 0.9f;
            Stats.AttackRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.stoppingDistance = newValue;
            };
        }

        protected override void MoveToTaget() =>  this.DefaultMoveToTaget();       

        protected override void Attack()
        {
            ITargetable target = TargetDetection.CurrentTarget;
            float damage = Stats.AttackDamage.FinalValue;
            CombatController.Instance.DamageDealing(target as ITakeDamageable, damage);
        }
    }
}
