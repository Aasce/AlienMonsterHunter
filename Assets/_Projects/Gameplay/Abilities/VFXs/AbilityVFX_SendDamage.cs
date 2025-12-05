using Asce.Game.Abilities;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.VFXs
{
    [RequireComponent(typeof(Ability))]
    public class AbilityVFX_SendDamage : GameComponent
    {
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField] private string _vfxName;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _ability);
        }
        private void Awake()
        {
            if (_ability is not ISendDamageAbility sendDamageAbility) return;
            sendDamageAbility.OnSendDamage += SendDamageAbility_OnSendDamage;
        }

        private void SendDamageAbility_OnSendDamage(Combats.DamageContainer container)
        {
            if (container == null) return;
            if (container.Receiver == null) return;
            if (container.Receiver is not GameComponent component) return;
            VFXObject vfx = VFXController.Instance.Spawn(_vfxName, component.transform.position, component.transform.eulerAngles.z);
            if (vfx == null) return;

        }
    }
}
