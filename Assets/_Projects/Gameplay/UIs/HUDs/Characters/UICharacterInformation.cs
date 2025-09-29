using Asce.Game.Entities;
using Asce.Game.Players;
using Asce.Game.Stats;
using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.HUDs
{
    public class UICharacterInformation : UIObject
    {
        [SerializeField] private UIResourceStatBar _healthBar;

        [Space]
        [SerializeField] private Character _character;

        public UIResourceStatBar HealthBar => _healthBar;

        public Character Character
        {
            get => _character;
            set
            {
                if (_character == value) return;
                this.Unregister();
                _character = value;
                this.Register();
            }
        }


        private void Start()
        {
            if (HealthBar == null) return;
            Player.Instance.OnCharacterChanged += Player_OnCharacterChanged;
        }

        private void Register()
        {
            if (Character == null)
            {
                HealthBar.ResourceStat = null;
                return;
            }
            HealthBar.ResourceStat = Character.Stats.Health;
        }

        private void Unregister()
        {
            if (Character == null) return;
        }


        private void Player_OnCharacterChanged(Character character)
        {
            Character = character;
        }

    }
}