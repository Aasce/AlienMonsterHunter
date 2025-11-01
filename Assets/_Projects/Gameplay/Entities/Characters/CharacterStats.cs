using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class  CharacterStats : EntityStats
    {
        [Header("Character Stats")]
        [SerializeField] protected Stat _selfViewRadius = new();
        [SerializeField] protected Stat _viewRadius = new();
        [SerializeField] protected Stat _viewAngle = new();

        public Stat SelfViewRadius => _selfViewRadius;
        public Stat ViewRadius => _viewRadius;
        public Stat ViewAngle => _viewAngle;


        public override void Initialize(SO_EntityStats baseStats)
        {
            base.Initialize(baseStats);
            _stats.Add(new StatContainer(nameof(SelfViewRadius), SelfViewRadius));
            _stats.Add(new StatContainer(nameof(ViewRadius), ViewRadius));
            _stats.Add(new StatContainer(nameof(ViewAngle), ViewAngle));

            if (baseStats is not SO_CharacterStats enemyStats) return;

            SelfViewRadius.Add(enemyStats.SelfViewRadius, StatValueType.Flat, StatSourceType.Base);
            ViewRadius.Add(enemyStats.ViewRadius, StatValueType.Flat, StatSourceType.Base);
            ViewAngle.Add(enemyStats.ViewAngle, StatValueType.Flat, StatSourceType.Base);
        }

        public override void ResetStats()
        {
            base.ResetStats();
        }

        protected override void ClearStats(Stats.StatSourceType sourceType = StatSourceType.Default)
        {
            base.ClearStats(sourceType);

            SelfViewRadius.Clear(sourceType);
            ViewRadius.Clear(sourceType);
            ViewAngle.Clear(sourceType);
        }
    }
}