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

        [Space]
        [SerializeField] protected Stat _viewRange = new ();

        public Stat AttackDamage => _attackDamage;
        public Stat AttackSpeed => _attackSpeed;
        public Stat AttackRange => _attackRange;

        public Stat ViewRange => _viewRange;


        public override void Initialize(SO_EntityStats baseStats)
        {
            _stats.Add(new StatContainer(nameof(AttackDamage), AttackDamage));
            _stats.Add(new StatContainer(nameof(AttackSpeed), AttackSpeed));
            _stats.Add(new StatContainer(nameof(AttackRange), AttackRange));
            _stats.Add(new StatContainer(nameof(ViewRange), ViewRange));

            base.Initialize(baseStats);
            if (baseStats is not SO_EnemyStats enemyStats) return;

            AttackDamage.Add(enemyStats.AttackDamage, StatValueType.Base);
            AttackSpeed.Add(enemyStats.AttackSpeed, StatValueType.Base);
            AttackRange.Add(enemyStats.AttackRange, StatValueType.Base);

            ViewRange.Add(enemyStats.ViewRange, StatValueType.Base);
        }

        public override void ResetStats()
        {
            base.ResetStats();
        }

        protected override void ClearStats(bool isClearBase = false)
        {
            base.ClearStats(isClearBase);
            AttackDamage.Clear(isClearBase);
            AttackSpeed.Clear(isClearBase);
            AttackRange.Clear(isClearBase);

            ViewRange.Clear(isClearBase);
        }
    }
}
