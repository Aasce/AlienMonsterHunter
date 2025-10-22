using Asce.Game.Entities;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class EffectController : MonoBehaviourSingleton<EffectController>
    {
        private readonly Dictionary<string, Pool<Effect>> _pools = new();

        public Effect CreateEffect(string effectName, EffectData data)
        {
            if (string.IsNullOrEmpty(effectName)) return null;

            Pool<Effect> pool = this.GetPool(effectName);
            if (pool == null) return null;

            Effect effect = pool.Activate(out bool isCreated);
            if (effect == null) return null;
            if (isCreated) effect.Initialize();
            else effect.ResetStatus();

            effect.SetData(data);
            return effect;
        }

        public Effect AddEffect(string effectName, Entity entity, EffectData data)
        {
            if (entity == null) return null;
            if (entity.IsDeath) return null;
            if (entity.Effects == null) return null;

            Effect effect = this.CreateEffect(effectName, data);

            effect.Receiver = entity;
            entity.Effects.Add(effect);
            effect.gameObject.SetActive(true);
            effect.Apply();
            return effect;
        }

        public bool RemoveEffect(Effect effect)
        {
            if (effect == null) return true;
            if (effect.Receiver == null) return false;
            if (effect.Receiver.Effects == null) return false;
            if (!effect.Receiver.Effects.Remove(effect)) return false;

            effect.Unpply();
            effect.gameObject.SetActive(false);
            Pool<Effect> pool = GetPool(effect.Information.Name);
            if (pool != null) pool.Deactivate(effect);
            return true;
        }

        private Pool<Effect> GetPool(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_pools.TryGetValue(name, out Pool<Effect> pool)) return pool;
            this.CreatePool(name); 
            if (_pools.TryGetValue(name, out pool)) return pool;
            return null;
        }

        private void CreatePool(string name)
        {
            Effect effectPrefab = GameManager.Instance.AllEffects.Get(name);
            if (effectPrefab == null)
            {
                Debug.LogWarning($"Effect with name: {name} not exist.");
                return;
            }

            if (!_pools.ContainsKey(name))
            {
                GameObject poolGameObject = new($"{name} Pool");
                poolGameObject.transform.SetParent(transform);

                Pool<Effect> newPool = new()
                {
                    Prefab = effectPrefab,
                    Parent = poolGameObject.transform,
                    IsSetActive = false,
                };
                _pools.Add(name, newPool);
            }
        }
    }

    [System.Serializable]
    public struct EffectData
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public float Strength
        {
            get => _strength;
            set => _strength = value;
        }
    }
}
