using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(EntityView))]
    public class HellFlamethrower_MachineView : GameComponent
    {
        [SerializeField, Readonly] private EntityView _view;
        [SerializeField, Readonly] private HellFlamethrower_Machine _machine;

        [Space]
        [SerializeField] private List<SpriteRenderer> _overheatParts = new();
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _overheatColor = Color.red;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _view);
            this.LoadComponent(out _machine);
        }

        private void LateUpdate()
        {
            if (_machine == null || _overheatParts.Count == 0) return;

            float t = 0f;

            if (!_machine.IsOverheat)
            {
                if (_machine.AttackToOverheat > 0)
                {
                    t = Mathf.Clamp01((float)_machine.CurrentAttack / _machine.AttackToOverheat);
                }
            }
            else t = Mathf.Clamp01(_machine.OverheatRecoveryCooldown.Ratio);
            
            Color targetColor = Color.Lerp(_normalColor, _overheatColor, t);
            foreach (SpriteRenderer renderer in _overheatParts)
            {
                if (renderer == null) continue;
                renderer.color = targetColor;
            }
        }
    }
}
