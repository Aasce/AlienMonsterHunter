using Asce.Game.Stats;
using System;

namespace Asce.Game.Combats
{
    public interface IHealable
    {
        public event Action<float> OnHealing;

        ResourceStat Health { get; }

        public void HealingCallback(float healAmount);
    }
}
