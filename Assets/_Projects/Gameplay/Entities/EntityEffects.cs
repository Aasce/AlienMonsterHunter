using Asce.Game.Effects;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityEffects : GameComponent, ISaveable<EffectsSaveData>, ILoadable<EffectsSaveData>
    {
        [SerializeField, Readonly] private Entity _entity;

        [Space]
        [SerializeField] private List<Effect> _effects = new();
        [SerializeField] private Cooldown _updateCooldown = new(0.1f);
        private ReadOnlyCollection<Effect> _effectsReadonly;

        public Entity Entity
        {
            get => _entity;
            set => _entity = value;
        }
        public ReadOnlyCollection<Effect> Effects => _effectsReadonly ??= _effects.AsReadOnly();


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _entity);
        }

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

        public Effect Get(string name)
        {
            foreach (Effect effect in _effects)
            {
                if (effect.Information.Name == name) return effect;
            }
            return null;
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

        public bool Contains(string name)
        {
            foreach (Effect effect in _effects)
            {
                if (effect.Information.Name == name) return true;
            }
            return false;
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

        EffectsSaveData ISaveable<EffectsSaveData>.Save()
        {
            EffectsSaveData data = new ();
            foreach (Effect effect in _effects)
            {
                EffectSaveData effectData = (effect as ISaveable<EffectSaveData>).Save();
                data.effects.Add(effectData);
            }
            return data;
        }

        void ILoadable<EffectsSaveData>.Load(EffectsSaveData data)
        {
            if (data ==  null) return;
            this.Clear();
            foreach (EffectSaveData effectData in data.effects)
            {
                Effect effect = EffectController.Instance.CreateEffect(effectData.name, new EffectData()
                {
                    Duration = effectData.baseDuration,
                    Strength = effectData.strength,
                });

                effect.Receiver = Entity;
                this.Add(effect);
                (effect as ILoadable<EffectSaveData>).Load(effectData);
                effect.gameObject.SetActive(true);
            }
        }
    }
}
