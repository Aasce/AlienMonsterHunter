using Asce.Game.Stats;
using System;

namespace Asce.Game.Combats
{
    public interface ITakeDamageable
    {
        public event Action<DamageContainer> OnBeforeTakeDamage;
        public event Action<DamageContainer> OnAfterTakeDamage;

        ResourceStat Health { get; }
        Stat Armor { get; }

        public void BeforeTakeDamageCallback(DamageContainer damage);
        public void AfterTakeDamageCallback(DamageContainer damage);
    }
}