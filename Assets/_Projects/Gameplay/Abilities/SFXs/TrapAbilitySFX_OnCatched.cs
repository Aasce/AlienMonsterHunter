using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class TrapAbilitySFX_OnCatched : GameComponent
    {
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField, Readonly] private SFXPlayer _sfxPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _sfxPlayer);
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
            _sfxPlayer.Play();
        }
    }
}
