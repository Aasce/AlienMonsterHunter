using System;

namespace Asce.Game.Stats
{
    public interface ITakeDamageable
    {
        public event Action<float> OnTakeDamage;

        ResourceStat Health { get; }
        Stat Armor { get; }

        public void TakeDamageCallback(float damage);
    }
}