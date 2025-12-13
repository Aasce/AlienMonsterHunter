using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class AbilitySFX_OnReactive : GameComponent
    {
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField, Readonly] private SFXPlayer _sfxPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _ability);
            this.LoadComponent(out _sfxPlayer);
        }

        private void Start()
        {
            _ability.OnReactive += Ability_OnReactive;
        }

        private void Ability_OnReactive(Vector2 position)
        {
            _sfxPlayer.Play();
        }
    }
}
