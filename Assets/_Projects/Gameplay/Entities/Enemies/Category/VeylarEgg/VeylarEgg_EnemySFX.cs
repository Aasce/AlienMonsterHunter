using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class VeylarEgg_EnemySFX : GameComponent
    {
        [SerializeField, Readonly] private VeylarEgg_Enemy _enemy;
        [SerializeField] private SFXPlayer _hatchSFXPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _enemy);
        }

        private void Start()
        {
            _enemy.OnHatched += Enemy_OnHatched;
        }

        private void Enemy_OnHatched()
        {
            _hatchSFXPlayer.Play();
        }
    }
}