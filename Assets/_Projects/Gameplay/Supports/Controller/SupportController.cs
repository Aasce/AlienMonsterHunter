using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class SupportController : MonoBehaviourSingleton<SupportController>
    {
        private readonly Dictionary<string, Pool<Support>> _pools = new();


        public List<Support> GetAllSupports()
        {
            List<Support> supports = new();
            var pools = _pools.Values;
            foreach (Pool<Support> pool in pools)
            {
                supports.AddRange(pool.Activities);
            }
            return supports;
        }

        public Support Spawn(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            if (!_pools.ContainsKey(id)) this.CreatePool(id);
            if (!_pools.TryGetValue(id, out Pool<Support> pool)) return null;

            Support support = pool.Activate(out bool isCreated);
            if (support == null) return null;
            if (isCreated) support.Initialize();
            else support.ResetStatus();

            support.OnSpawn();
            return support;
        }

        public void Despawn(Support support)
        {
            if (support == null) return;
            if (support.Information == null) return;
            if (_pools.TryGetValue(support.Information.Name, out Pool<Support> pool))
            {
                pool.Deactivate(support);
                support.gameObject.SetActive(false);
                return;
            }
            GameObject.Destroy(support);
        }

        private void CreatePool(string id)
        {
            Support supportPrefab = GameManager.Instance.AllSupports.Get(id);
            if (supportPrefab == null) return;

            GameObject poolParent = new GameObject($"{id} Pool");
            poolParent.transform.SetParent(this.transform);

            Pool<Support> pool = new()
            {
                Prefab = supportPrefab,
                Parent = poolParent.transform,
                IsSetActive = false
            };
            _pools[id] = pool;
        }
    }
}
