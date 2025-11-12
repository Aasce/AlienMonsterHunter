using Asce.Game.UIs.Worlds;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [RequireComponent(typeof(CharacterAbility))]
    public class CharacterAbility_DespawnCooldownToMachineUI : GameComponent
    {
        [SerializeField, Readonly] private CharacterAbility _characterAbility;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _characterAbility);
        }

        private void Start()
        {
            if (_characterAbility is not IControlMachineAbility controlMachineAbility) return;
            if (!controlMachineAbility.Machine.UI.TryGetComponent(out MachineUI_DespawnTime despawnTimeUI)) return;

            despawnTimeUI.DespawnCooldown = _characterAbility.DespawnTime;
        }
    }
}
