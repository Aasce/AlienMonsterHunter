using Asce.Game.Combats;
using System;

namespace Asce.Game.Abilities
{
    public interface ISendDamageAbility
    {
        public event Action<DamageContainer> OnSendDamage;

        public float Damage { get; }

    }
}