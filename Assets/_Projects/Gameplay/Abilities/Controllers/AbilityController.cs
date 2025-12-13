using Asce.Game.Managers;
using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class AbilityController : MonoBehaviourSingleton<AbilityController>
    {
        private readonly Dictionary<string, Pool<Ability>> _pools = new();

        [SerializeField] private Cooldown _updateCooldown = new(0.1f);


        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (_updateCooldown.IsComplete)
            {
                var pools = _pools.Values;
                foreach (Pool<Ability> pool in pools)
                {
                    if (pool == null) continue;
                    pool.DeactivateMatch(this.DeactiveAbilities);
                }
                _updateCooldown.Reset();
            }
        }

        public List<Ability> GetAllAbilities()
        {
            List<Ability> abilities = new();
            var pools = _pools.Values;
            foreach (Pool<Ability> pool in pools)
            {
                if (pool == null) continue;
                abilities.AddRange(pool.Activities);
            }
            return abilities;
        }

        public Ability Spawn(string name, GameObject owner)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (!_pools.ContainsKey(name)) this.CreatePool(name);
            if (!_pools.TryGetValue(name, out Pool<Ability> pool)) return null;

            Ability ability = pool.Activate(out bool isCreated);
            if (ability == null) return null;
            if (isCreated) ability.Initialize();
            else ability.ResetStatus();

            ability.Owner = owner;
            ability.Spawn();
            return ability;
        }


        private bool DeactiveAbilities(Ability ability)
        {
            if (ability == null) return true;

            ability.DespawnTime.Update(_updateCooldown.BaseTime);
            if (ability.DespawnTime.IsComplete)
            {
                ability.Despawn();
                ability.gameObject.SetActive(false);
                return true;
            }
            return false;
        }

        private void CreatePool(string name)
        {
            Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(name);
            if (abilityPrefab == null) return;

            GameObject poolParent = new GameObject($"{name} Pool");
            poolParent.transform.SetParent(this.transform);

            Pool<Ability> pool = new()
            {
                Prefab = abilityPrefab,
                Parent = poolParent.transform,
                IsSetActive = false
            };
            _pools[name] = pool;
        }
    }
}