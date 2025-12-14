using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Veylar_EnemySFX : GameComponent
    {
        [SerializeField, Readonly] private Veylar_Enemy _enemy;
        [SerializeField] private SFXPlayer _explosionSFXPlayer;
        [SerializeField] private SFXPlayer _laySFXPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _enemy);
        }

        private void Start()
        {
            _enemy.OnDead += Enemy_OnDead;
            _enemy.OnLay += Enemy_OnLay;
        }

        private void Enemy_OnDead(Combats.DamageContainer container)
        {
            _explosionSFXPlayer.Play();
        }

        private void Enemy_OnLay()
        {
            _laySFXPlayer.Play();
        }

    }
}
