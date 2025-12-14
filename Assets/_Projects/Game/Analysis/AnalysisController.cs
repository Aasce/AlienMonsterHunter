using Asce.Core;
using Asce.Core.Attributes;
using Asce.MainGame.Managers;
using UnityEngine;

namespace Asce.MainGame.Analysis
{
    public class AnalysisController : ControllerComponent
    {
        [SerializeField, Readonly] private float _totalDamageDealed = 0f;
        [SerializeField, Readonly] private float _totalHeal = 0f;

        [Space]
        [SerializeField, Readonly] private float _totalDamageReceived = 0f;

        public override string ControllerName => "Analysis";

        public override void Ready()
        {
            base.Ready();
            MainGameManager.Instance.Player.Character.OnAfterSendDamage += Character_OnAfterSendDamage;
            MainGameManager.Instance.Player.Character.OnHealing += Character_OnHealing;

            MainGameManager.Instance.Player.Character.OnAfterTakeDamage += Character_OnAfterTakeDamage;
        }

        private void Character_OnAfterSendDamage(Game.Combats.DamageContainer container)
        {
            _totalDamageDealed += container.FinalDamage;
        }

        private void Character_OnHealing(float healAmount)
        {
            _totalHeal += healAmount;
        }

        private void Character_OnAfterTakeDamage(Game.Combats.DamageContainer container)
        {
            _totalDamageReceived += container.FinalDamage;
        }

    }
}
