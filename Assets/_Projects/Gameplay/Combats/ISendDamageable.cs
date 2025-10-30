using System;

namespace Asce.Game.Combats
{
    public interface ISendDamageable
    {
        public event Action<DamageContainer> OnBeforeSendDamage;
        public event Action<DamageContainer> OnAfterSendDamage;

        public void BeforeSendDamageCallback(DamageContainer damage);
        public void AfterSendDamageCallback(DamageContainer damage);
    }
}
