using Asce.Game.Entities.Machines;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Worlds
{
    [RequireComponent(typeof(MachineUI))]
    public class MachineUI_DespawnTime : UIWorldObject
    {
        [SerializeField, Readonly] private MachineUI _machineUI;
        [SerializeField] private Slider _despawnTimeSlider;

        [Header("Runtime")]
        [SerializeField, Readonly] private Cooldown _despawnCooldown;

        public Cooldown DespawnCooldown
        {
            get => _despawnCooldown;
            set 
            {
                if (_despawnCooldown == value) return;
                this.UnRegister();
                _despawnCooldown = value;
                this.Register();

            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _machineUI);
        }

        private void Register()
        {
            if (_despawnCooldown == null) return;

            _despawnTimeSlider.maxValue = _despawnCooldown.BaseTime;
            _despawnTimeSlider.value = _despawnCooldown.CurrentTime;

            _despawnCooldown.OnBaseTimeChanged += DespawnCooldown_OnBaseTimeChanged;
            _despawnCooldown.OnCurrentTimeChanged += DespawnCooldown_OnCurrentTimeChanged;
        }

        private void UnRegister()
        {
            if (_despawnCooldown == null) return;

            _despawnCooldown.OnBaseTimeChanged -= DespawnCooldown_OnBaseTimeChanged;
            _despawnCooldown.OnCurrentTimeChanged -= DespawnCooldown_OnCurrentTimeChanged;
        }

        private void DespawnCooldown_OnBaseTimeChanged(object sender, float newValue)
        {
            _despawnTimeSlider.maxValue = newValue;
        }

        private void DespawnCooldown_OnCurrentTimeChanged(object sender, float newValue)
        {
            _despawnTimeSlider.value = newValue;
        }

    }
}
