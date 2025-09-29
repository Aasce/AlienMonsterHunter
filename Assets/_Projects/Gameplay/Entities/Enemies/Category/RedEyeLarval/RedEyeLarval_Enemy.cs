using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class RedEyeLarval_Enemy : Enemy
    {
        protected override void Start()
        {
            base.Start();
            Agent.stoppingDistance = Stats.AttackRange.FinalValue * 0.9f;
            Stats.AttackRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.stoppingDistance = newValue;
            };
        }

        protected override void MoveToTaget() =>  this.DefaultMoveToTaget();       

        protected override void FindTarget()
        {
            this.DefaultFindTarget();
            if (Target != null) this.MoveToTaget();
        }

        protected override void Attack()
        {
            float damage = Stats.AttackDamage.FinalValue;
            CombatController.Instance.DamageDealing(_target, damage);
        }
    }
}
