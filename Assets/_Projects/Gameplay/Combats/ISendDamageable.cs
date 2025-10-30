using System;

namespace Asce.Game.Combats
{
    public interface ISendDamageable
    {
        public event Action<DamageContainer> OnBeforeSendDamage;
        public event Action<DamageContainer> OnAfterSendDamage;
        public event Action<DamageContainer> OnKill;


        public void BeforeSendDamageCallback(DamageContainer container);
        public void AfterSendDamageCallback(DamageContainer container);
        public void KillCallback(DamageContainer container);
    }
}
