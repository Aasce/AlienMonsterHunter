using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class AbilityController : MonoBehaviourSingleton<AbilityController>
    {
        private readonly Dictionary<string, Pool<Ability>> _pools = new();

        [Space]
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

        public Ability Spawn(string name, GameObject owner)
        {
            if (string.IsNullOrEmpty(name)) return null;

            Ability abilityPrefab = GameManager.Instance.AllAbilities.Get(name);
            if (abilityPrefab == null) return null;

            if (!_pools.ContainsKey(name))
            {
                Pool<Ability> newPool = new()
                {
                    Prefab = abilityPrefab,
                    Parent = this.transform,
                    IsSetActive = false,
                };
                _pools.Add(name, newPool);
            }

            Pool<Ability> pool = _pools[name];
            Ability ability = pool.Activate(out bool isCreated);
            if (ability == null) return ability;
            if (!isCreated)
            {
                ability.gameObject.SetActive(true);
            }

            ability.Owner = owner;
            ability.DespawnTime.Reset();
            ability.OnSpawn();
            return ability;
        }


        private bool DeactiveAbilities(Ability ability)
        {
            if (ability == null) return true;

            ability.DespawnTime.Update(_updateCooldown.BaseTime);
            if (ability.DespawnTime.IsComplete)
            {
                ability.OnDespawn();
                ability.gameObject.SetActive(false);
                return true;
            }
            return false;
        }

    }
}