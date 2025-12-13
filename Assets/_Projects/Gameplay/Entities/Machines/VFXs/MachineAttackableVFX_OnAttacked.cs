using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.VFXs;
using System.Net;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class MachineAttackableVFX_OnAttacked : GameComponent
    {
        [SerializeField, Readonly] private Machine _machine;
        [SerializeField, Readonly] private VFXSpawner _vfxSpawner;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _vfxSpawner);
            if (this.LoadComponent(out _machine))
            {
                if (_machine is not IMachineAttackable)
                    Debug.Log("[MachineFireableSFX_OnFired] Machine is not IMachineAttackable", this);
            }
        }

        private void Start()
        {
            if (_machine is IMachineAttackable attackable)
            {
                attackable.OnAttacked += MachineAttackable_OnAttacked;
            }
        }

        private void MachineAttackable_OnAttacked(ITargetable target)
        {
            _vfxSpawner.Spawn(transform.position, target.transform.position);
        }
    }
}
