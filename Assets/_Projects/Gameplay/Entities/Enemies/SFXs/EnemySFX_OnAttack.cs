using UnityEngine;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;

namespace Asce.Game.Entities.Enemies
{
    public class EnemySFX_OnAttack : GameComponent
    {
        [SerializeField, Readonly] private Enemy _enemy;
        [SerializeField] private SFXPlayer _sfxPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out  _enemy);
        }

        private void Start()
        {
            _enemy.OnAttacked += Enemy_OnAttacked;
        }

        private void Enemy_OnAttacked()
        {
            _sfxPlayer.Play();
        }
    }
}