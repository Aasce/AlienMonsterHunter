using Asce.Game.Stats;
using System;

namespace Asce.Game.Combats
{
    public interface ITakeDamageable
    {
        public event Action<DamageContainer> OnBeforeTakeDamage;
        public event Action<DamageContainer> OnAfterTakeDamage;
        public event Action<DamageContainer> OnDead;

        public bool IsDeath { get; }
        ResourceStat Health { get; }
        Stat Armor { get; }

        public void BeforeTakeDamageCallback(DamageContainer container);
        public void AfterTakeDamageCallback(DamageContainer container);
        public void DeadCallback(DamageContainer container);
    }
}