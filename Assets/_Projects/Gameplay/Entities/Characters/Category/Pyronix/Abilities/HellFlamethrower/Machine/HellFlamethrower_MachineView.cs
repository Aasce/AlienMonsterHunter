using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(EntityView))]
    public class HellFlamethrower_MachineView : GameComponent
    {
        [SerializeField, Readonly] private EntityView _view;
        [SerializeField, Readonly] private HellFlamethrower_Machine _machine;

        [Space]
        [SerializeField] private List<SpriteRenderer> _overheatParts = new(); 
        [SerializeField] private Gradient _overheatGradient;

        [Space]
        [SerializeField] private VisualEffect _flameEffect;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _view);
            this.LoadComponent(out _machine);
        }

        private void Start()
        {
            _flameEffect.Stop();
            _flameEffect.transform.rotation = Quaternion.Euler(0f, 0f, _machine.Angle);

            _machine.OnRotated += Machine_OnRotated;
            _machine.OnSprayStateChanged += Machine_OnSprayStateChanged;
            _machine.OnDead += Machine_OnDead;
        }
        private void LateUpdate()
        {
            if (_machine == null || _overheatParts.Count == 0 || _overheatGradient == null)
                return;

            float t = 0f;

            if (!_machine.IsOverheat)
            {
                if (_machine.AttackToOverheat > 0)
                {
                    t = Mathf.Clamp01(
                        (float)_machine.CurrentAttack / _machine.AttackToOverheat
                    );
                }
            }
            else
            {
                // When overheated, go backward / recovery phase
                t = Mathf.Clamp01(_machine.OverheatRecoveryCooldown.Ratio);
            }

            Color targetColor = _overheatGradient.Evaluate(t);

            foreach (SpriteRenderer renderer in _overheatParts)
            {
                if (renderer == null) continue;
                renderer.color = targetColor;
            }
        }


        private void Machine_OnRotated(float angle)
        {
            _flameEffect.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Machine_OnSprayStateChanged(bool state)
        {
            if (state) _flameEffect.Play();
            else _flameEffect.Stop();
        }

        private void Machine_OnDead(Combats.DamageContainer container)
        {
            _flameEffect.Stop();
        }
    }
}
