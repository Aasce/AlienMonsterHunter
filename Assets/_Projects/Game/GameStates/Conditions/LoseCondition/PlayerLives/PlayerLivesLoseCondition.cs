using Asce.Game.Players;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class PlayerLivesLoseCondition : LoseCondition
    {
        [SerializeField] private int _maxLives = 3;
        [SerializeField] private int _currentLives = 3;

        public override void Initialize()
        {
            base.Initialize();
            _currentLives = _maxLives;
            if (Player.Instance.Character != null) Player.Instance.Character.OnDead += Character_OnDead;
            Player.Instance.OnCharacterChanged += Player_OnCharacterChanged;
        }

        public override bool IsSatisfied()
        {
            return _currentLives <= 0;
        }

        private void Character_OnDead(Combats.DamageContainer container)
        {
            _currentLives--;
        }

        private void Player_OnCharacterChanged(ValueChangedEventArgs<Entities.Characters.Character> eventArgs)
        {
            if (eventArgs.OldValue != null)
            {
                eventArgs.OldValue.OnDead -= Character_OnDead;
            }

            if (eventArgs.NewValue != null)
            {
                eventArgs.NewValue.OnDead += Character_OnDead;
            }
        }
    }
}
