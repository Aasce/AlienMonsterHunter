using Asce.Game.Combats;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class RedEyeLarval_Enemy : Enemy
    {
        public override void Initialize()
        {
            base.Initialize();
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
