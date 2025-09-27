using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyStats : EntityStats
    {
        [Header("Enemy Stats")]
        [SerializeField] protected Stat _attackDamage = new ();
        [SerializeField] protected Stat _attackSpeed = new ();
        [SerializeField] protected Stat _attackRange = new ();

        public Stat AttackDamage => _attackDamage;
        public Stat AttackSpeed => _attackSpeed;
        public Stat AttackRange => _attackRange;


        public override void Initialize(SO_EntityStats baseStats)
        {
            base.Initialize(baseStats);
            if (baseStats is not SO_EnemyStats enemyStats) return;

            AttackDamage.Add(enemyStats.AttackDamage, StatValueType.Base);
            AttackSpeed.Add(enemyStats.AttackSpeed, StatValueType.Base);
            AttackRange.Add(enemyStats.AttackRange, StatValueType.Base);
        }

        public override void ResetStats()
        {
            base.ResetStats();
        }

        protected override void ClearStats()
        {
            base.ClearStats();
            AttackDamage.Clear();
            AttackSpeed.Clear();
            AttackRange.Clear();
        }
    }
}
