using Asce.Game.Entities.Characters;
using Asce.Game.UIs;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs.HUDs
{
    public class UICharacterInformation : UIObject
    {
        [SerializeField] private Image _avatar;

        [SerializeField] private UIExpLeveling _leveling;
        [SerializeField] private UIResourceStatBar _healthBar;
        [SerializeField] private UICharacterAbilities _abilities;
        [SerializeField] private UIEffects _effects;

        [Space]
        [SerializeField, Readonly] private Character _character;

        public UIExpLeveling Leveling => _leveling;
        public UIResourceStatBar HealthBar => _healthBar;
        public UICharacterAbilities Abilities => _abilities;
        public UIEffects Effects => _effects;

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
                Leveling.ExpLeveling = null;
                HealthBar.ResourceStat = null;
                Abilities.CharacterAbilities = null;
                Effects.EntityEffects = null;
                return;
            }

            Leveling.ExpLeveling = Character.Leveling;
            HealthBar.ResourceStat = Character.Stats.Health;
            Abilities.CharacterAbilities = Character.Abilities;
            Effects.EntityEffects = Character.Effects;
        }

        private void Unregister()
        {
            if (Character == null) return;
        }
    }
}