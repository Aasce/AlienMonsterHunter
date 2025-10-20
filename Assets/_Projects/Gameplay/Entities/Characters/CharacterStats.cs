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
            _stats.Add(new StatContainer(nameof(SelfViewRadius), SelfViewRadius));
            _stats.Add(new StatContainer(nameof(ViewRadius), ViewRadius));
            _stats.Add(new StatContainer(nameof(ViewAngle), ViewAngle));

            base.Initialize(baseStats);
            if (baseStats is not SO_CharacterStats enemyStats) return;

            SelfViewRadius.Add(enemyStats.SelfViewRadius, StatValueType.Base);
            ViewRadius.Add(enemyStats.ViewRadius, StatValueType.Base);
            ViewAngle.Add(enemyStats.ViewAngle, StatValueType.Base);
        }

        public override void ResetStats()
        {
            base.ResetStats();
        }

        protected override void ClearStats(bool isClearBase = false)
        {
            base.ClearStats(isClearBase);

            SelfViewRadius.Clear(isClearBase);
            ViewRadius.Clear(isClearBase);
            ViewAngle.Clear(isClearBase);
        }
    }
}