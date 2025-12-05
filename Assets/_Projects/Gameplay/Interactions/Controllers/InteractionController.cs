using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public class InteractionController : MonoBehaviourSingleton<InteractionController>
    {
        private readonly Dictionary<string, Pool<InteractiveObject>> _pools = new();

        public List<InteractiveObject> GetAllInteractiveObjects()
        {
            List<InteractiveObject> objects = new();
            var pools = _pools.Values;
            foreach (Pool<InteractiveObject> pool in pools)
            {
                if (pool == null) continue;
                objects.AddRange(pool.Activities);
            }
            return objects;
        }

        public InteractiveObject Spawn(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (!_pools.ContainsKey(name)) this.CreatePool(name);
            if (!_pools.TryGetValue(name, out Pool<InteractiveObject> pool)) return null;

            InteractiveObject interactiveObject = pool.Activate(out bool isCreated);
            if (interactiveObject == null) return null;
            if (isCreated) interactiveObject.Initialize();
            else interactiveObject.ResetStatus();

            interactiveObject.gameObject.SetActive(true);
            interactiveObject.OnSpawn();
            return interactiveObject;
        }

        public void Despawn(InteractiveObject interactiveObject)
        {
            if (interactiveObject == null) return;
            if (interactiveObject.Information == null) return;
            if (_pools.TryGetValue(interactiveObject.Information.Name, out Pool<InteractiveObject> pool))
            {
                pool.Deactivate(interactiveObject);
                interactiveObject.gameObject.SetActive(false);
                return;
            }
            GameObject.Destroy(interactiveObject);
        }

        private void CreatePool(string name)
        {
            InteractiveObject interactiveObjectPrefab = GameManager.Instance.AllInteractiveObjects.Get(name);
            if (interactiveObjectPrefab == null) return;

            GameObject poolParent = new GameObject($"{name} Pool");
            poolParent.transform.SetParent(this.transform);

            Pool<InteractiveObject> pool = new()
            {
                Prefab = interactiveObjectPrefab,
                Parent = poolParent.transform,
                IsSetActive = false
            };
            _pools[name] = pool;
        }
    }
}
