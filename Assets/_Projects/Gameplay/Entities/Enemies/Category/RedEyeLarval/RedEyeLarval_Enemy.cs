using Asce.Game.Combats;
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
            float penetration = Information.Stats.GetCustomStat("Attack Penetration");
            CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
            {
                Damage = damage,
                Penetration = penetration,
            });
        }
    }
}
