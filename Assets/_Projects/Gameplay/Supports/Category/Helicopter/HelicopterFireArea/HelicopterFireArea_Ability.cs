using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HelicopterFireArea_Ability : Ability
    {
        private readonly HashSet<ITargetable> _targets = new();
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private Cooldown _updateCooldown = new(0.5f);

        [SerializeField] private float _igniteDamage = 5f;

        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (_updateCooldown.IsComplete)
            {
                _updateCooldown.Reset();
                UpdateEffects();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.enabled) return;
            if (collision.gameObject == Owner) return;
            if (!LayerUtils.IsInLayerMask(collision, _targetLayer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (_targets.Contains(target)) return;

            _targets.Add(target);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.enabled) return;
            if (collision.gameObject == Owner) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _targetLayer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;

            _targets.Remove(target);
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            foreach (ITargetable target in _targets)
            {
                if (target is not Entity entity) continue;
                Effect effect = entity.Effects.Get("Helicopter Ignite");
                EffectController.Instance.RemoveEffect(effect);
            }
        }

        private void UpdateEffects()
        {
            foreach (ITargetable target in _targets)
            {
                if (target is not Entity entity) continue;
                Effect effect = entity.Effects.Get("Helicopter Ignite");
                if (!target.IsTargetable) EffectController.Instance.RemoveEffect(effect);
                
                if (effect != null)
                {
                    effect.Duration.Reset();
                }
                else
                {
                    EffectController.Instance.AddEffect("Helicopter Ignite", entity, new EffectData()
                    {
                        Strength = _igniteDamage,
                        Duration = 10f
                    });
                }
            }
        }

    }
}
