using Asce.Game.Effects;
using Asce.Game.SaveLoads;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityEffects : GameComponent, ISaveable<EffectsSaveData>, ILoadable<EffectsSaveData>
    {
        [SerializeField, Readonly] private Entity _entity;

        [Space]
        [SerializeField, Readonly] private List<Effect> _effects = new();
        [SerializeField] private Cooldown _updateCooldown = new(0.1f);
        private ReadOnlyCollection<Effect> _effectsReadonly;

        [Space]
        [SerializeField, Readonly] private List<EffectStatContainer> _effectStats = new();
        [SerializeField] private EffectStat _untargetable = new();
        [SerializeField] private EffectStat _unmoveable = new();
        [SerializeField] private EffectStat _unattackable = new();

        public event Action<Effect> OnEffectAdded;
        public event Action<Effect> OnEffectRemoved;

        public Entity Entity
        {
            get => _entity;
            set => _entity = value;
        }
        public ReadOnlyCollection<Effect> Effects => _effectsReadonly ??= _effects.AsReadOnly();

        public EffectStat Untargetable => _untargetable;
        public EffectStat Unmoveable => _unmoveable;
        public EffectStat Unattackable => _unattackable;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _entity);
        }

        public virtual void Initialize()
        {
            _effectStats.Add(new ("Untargetable", _untargetable));
            _effectStats.Add(new ("Unmoveable", _unmoveable));
            _effectStats.Add(new ("Unattackable", _unattackable));
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
            OnEffectAdded?.Invoke(effect);
        }

        public bool Remove(Effect effect) 
        {
            bool isRemoved = _effects.Remove(effect);
            if (isRemoved) OnEffectRemoved?.Invoke(effect);
            return isRemoved;
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

            foreach (EffectStatContainer container in _effectStats)
            {
                EffectStat effectStat = container.EffectStat;
                if (effectStat == null) continue;
                effectStat.ClearAll();
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

            foreach (EffectStatContainer container in _effectStats)
            {
                if (container.EffectStat is ISaveable<EffectStatSaveData> saveable)
                {
                    EffectStatSaveData effectData = saveable.Save();
                    data.effectStats.Add(new EffectStatContainerSaveData(container.Name, effectData));
                }
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

            foreach (EffectStatContainerSaveData effectStatData in data.effectStats)
            {
                EffectStatContainer container = _effectStats.Find((c) => c.Name == effectStatData.Name);
                if (container.EffectStat is ILoadable<EffectStatSaveData> loadable)
                {
                    loadable.Load(effectStatData.EffectStat);
                }
            }
        }
    }
}
