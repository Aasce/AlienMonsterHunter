using Asce.Game.Entities.Characters;
using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.HUDs
{
    public class UICharacterInformation : UIObject
    {
        [SerializeField] private Image _avatar;

        [SerializeField] private UIExpLeveling _leveling;
        [SerializeField] private UIResourceStatBar _healthBar;
        [SerializeField] private UICharacterAbilities _abilities;

        [Space]
        [SerializeField] private Character _character;

        public UIExpLeveling Leveling => _leveling;
        public UIResourceStatBar HealthBar => _healthBar;
        public UICharacterAbilities Abilities => _abilities;

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
        }

        private void Register()
        {
            if (Character == null)
            {
                HealthBar.ResourceStat = null;
                Abilities.CharacterAbilities = null;
                Leveling.ExpLeveling = null;
                return;
            }
            HealthBar.ResourceStat = Character.Stats.Health;
            Abilities.CharacterAbilities = Character.Abilities;
            Leveling.ExpLeveling = Character.Leveling;
        }

        private void Unregister()
        {
            if (Character == null) return;
        }
    }
}