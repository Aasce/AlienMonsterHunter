using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Players;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class PlayerLivesLoseCondition : GameStateCondition
    {
        [SerializeField] private int _maxLives = 3;
        [SerializeField] private int _currentLives = 3;

        public override string ConditionName => "Player Lives";

        public override void Ready()
        {
            base.Ready();
            _currentLives = _maxLives;
            if (PlayerManager.Instance.Player is IPlayerControlCharacter controllCharacter)
            {
                if (controllCharacter.Character != null) controllCharacter.Character.OnDead += Character_OnDead;
                controllCharacter.OnCharacterChanged += Player_OnCharacterChanged;
            }
        }

        public override void SetData(MapLevelGameStateCondition data)
        {
            base.SetData(data);
            _maxLives = Mathf.RoundToInt(data.Get("MaxLives"));
        }

        public override bool IsSatisfied()
        {
            return _currentLives <= 0;
        }

        private void Character_OnDead(Game.Combats.DamageContainer container)
        {
            _currentLives--;
        }

        private void Player_OnCharacterChanged(ValueChangedEventArgs<Game.Entities.Characters.Character> eventArgs)
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


        protected override void OnBeforeSave(GameStateConditionSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("MaxLives", _maxLives);
            data.SetCustom("CurrentLives", _currentLives);
        }

        protected override void OnAfterLoad(GameStateConditionSaveData data)
        {
            base.OnAfterLoad(data);
            _maxLives = data.GetCustom<int>("MaxLives");
            _currentLives = data.GetCustom<int>("CurrentLives");
        }
    }
}
