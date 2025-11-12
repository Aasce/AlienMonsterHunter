using Asce.Game.Entities;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class EffectController : MonoBehaviourSingleton<EffectController>
    {
        private readonly Dictionary<string, Pool<Effect>> _pools = new();

        public Effect CreateEffect(string effectName, EffectData data)
        {
            if (string.IsNullOrEmpty(effectName)) return null;

            Pool<Effect> pool = GetPool(effectName);
            if (pool == null) return null;

            Effect effect = pool.Activate(out bool isCreated);
            if (effect == null) return null;

            if (isCreated) effect.Initialize();
            else effect.ResetStatus();

            effect.SetData(data);
            return effect;
        }

        public Effect AddEffect(string effectName, Entity sender, Entity receiver, EffectData data)
        {
            if (receiver == null || receiver.IsDeath || receiver.Effects == null) return null;

            Effect effectPrefab = GameManager.Instance.AllEffects.Get(effectName);
            if (effectPrefab == null)
            {
                Debug.LogWarning($"Effect with name: \"{effectName}\" not exist.");
                return null;
            }

            return effectPrefab.Information.ApplyType switch
            {
                EffectApplyType.Stacking => HandleStackingEffect(effectName, sender, receiver, data, effectPrefab),
                EffectApplyType.ResetDuration => HandleResetDurationEffect(effectName, sender, receiver, data),
                _ => AddNewEffect(effectName, sender, receiver, data),
            };
        }

        public bool RemoveEffect(Effect effect)
        {
            if (effect == null) return true;
            if (effect.Receiver == null || effect.Receiver.Effects == null) return false;
            if (!effect.Receiver.Effects.Remove(effect)) return false;

            effect.Unapply();
            effect.gameObject.SetActive(false);

            Pool<Effect> pool = GetPool(effect.Information.Name);
            pool?.Deactivate(effect);
            return true;
        }

        private Effect AddNewEffect(string effectName, Entity sender, Entity receiver, EffectData data)
        {
            Effect effect = CreateEffect(effectName, data);
            if (effect == null) return null;

            effect.Sender = sender;
            effect.Receiver = receiver;
            effect.gameObject.SetActive(true);

            receiver.Effects.Add(effect);
            effect.Apply();

            return effect;
        }

        private Pool<Effect> GetPool(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_pools.TryGetValue(name, out Pool<Effect> pool)) return pool;

            CreatePool(name);
            _pools.TryGetValue(name, out pool);
            return pool;
        }

        private void CreatePool(string name)
        {
            Effect effectPrefab = GameManager.Instance.AllEffects.Get(name);
            if (effectPrefab == null)
            {
                Debug.LogWarning($"Effect with name: {name} not exist.");
                return;
            }

            if (_pools.ContainsKey(name)) return;

            GameObject poolGO = new($"{name} Pool");
            poolGO.transform.SetParent(transform);

            Pool<Effect> newPool = new()
            {
                Prefab = effectPrefab,
                Parent = poolGO.transform,
                IsSetActive = false
            };

            _pools.Add(name, newPool);
        }

        #region --- ApplyType Handlers ---

        private Effect HandleStackingEffect(string effectName, Entity sender, Entity receiver, EffectData data, Effect prefab)
        {
            // Not stackable
            if (!prefab.Information.IsStackable)
                return AddNewEffect(effectName, sender, receiver, data);

            Effect existStackEffect = receiver.Effects.Effects.FirstOrDefault(e => e.Information.Name == effectName);
            if (existStackEffect == null)
                return AddNewEffect(effectName, sender, receiver, data);

            existStackEffect.ApplyStack(data);
            return existStackEffect;
        }

        private Effect HandleResetDurationEffect(string effectName, Entity sender, Entity receiver, EffectData data)
        {
            Effect existEffect = receiver.Effects.Effects.FirstOrDefault(e => e.Information.Name == effectName);
            if (existEffect == null)
                return AddNewEffect(effectName, sender, receiver, data);

            existEffect.Duration.Reset();
            return existEffect;
        }

        #endregion

    }
}
