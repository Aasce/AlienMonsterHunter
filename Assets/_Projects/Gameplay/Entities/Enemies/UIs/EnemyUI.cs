using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyUI : EntityUI
    {
        [SerializeField, Readonly] private Cooldown _hideCooldown = new(3f);

        public new Enemy Owner
        {
            get => base.Owner as Enemy;
            set => base.Owner = value;
        }

        public override void Initialize()
        {
            base.Initialize();
            _hideCooldown.ToComplete();
            _canvas.gameObject.SetActive(false);
            Owner.OnAfterTakeDamage += Owner_OnAfterTakeDamage;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _hideCooldown.ToComplete();
            _canvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!Owner.IsTargetable)
            {
                _canvas.gameObject.SetActive(false);
                return;
            }

            if (_hideCooldown.IsComplete) return;

            _hideCooldown.Update(Time.deltaTime);
            if (_hideCooldown.IsComplete)
            {
                _canvas.gameObject.SetActive(false);
            }
        }

        private void Owner_OnAfterTakeDamage(Combats.DamageContainer container)
        {
            _hideCooldown.Reset();
            if (!Owner.IsTargetable) return;
            _canvas.gameObject.SetActive(true);
        }
    }
}
