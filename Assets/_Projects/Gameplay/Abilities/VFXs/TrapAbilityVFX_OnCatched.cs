using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.VFXs;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class TrapAbilityVFX_OnCatched : GameComponent
    {
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField] private string _vfxName;

        protected override void RefReset()
        {
            base.RefReset();
            if (this.LoadComponent(out _ability))
            {
                if (_ability is not ITrapAbility)
                    Debug.Log("[TrapAbilityVFX_OnCatching] Ability is not ITrapAbility", this);
            }
        }

        private void Start()
        {
            if (_ability is ITrapAbility trapAbility)
            {
                trapAbility.OnCatched += TrapAbility_OnCatched;
            }
        }

        private void TrapAbility_OnCatched()
        {
            VFXController.Instance.Spawn(_vfxName, transform.position);
        }
    }
}
