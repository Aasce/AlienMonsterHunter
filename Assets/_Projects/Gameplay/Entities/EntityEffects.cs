using Asce.Game.Effects;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityEffects : GameComponent
    {
        [SerializeField] private List<Effect> _effects = new();

        [SerializeField] private Cooldown _updateCooldown = new(0.1f);

        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (!_updateCooldown.IsComplete) return;
            _updateCooldown.Reset();

            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                Effect effect = _effects[i];
                if (effect == null) continue;
                effect.Duration.Update(_updateCooldown.BaseTime);
                if (effect.Duration.IsComplete) EffectController.Instance.RemoveEffect(effect);
            }
        }

        public void Add(Effect effect)
        {
            if (effect == null) return;
            _effects.Add(effect);
        }

        public bool Remove(Effect effect) 
        {
            return _effects.Remove(effect);
        }

        public void Clear()
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                Effect effect = _effects[i];
                if (effect == null) continue;
                EffectController.Instance.RemoveEffect(effect);
            }
        }
    }
}
