using Asce.Managers;
using Asce.Managers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class VFXController : MonoBehaviourSingleton<VFXController>
    {
        [SerializeField] private SO_VFXs _vfxs;
        private Dictionary<string, Pool<VFXObject>> _pools = new();


        public VFXObject Spawn(string name, Vector2 position, float angle = 0f)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_vfxs == null) return null;

            if (!_pools.ContainsKey(name)) this.CreatePool(name);
            if (!_pools.TryGetValue(name, out Pool<VFXObject> pool)) return null;

            VFXObject vfx = pool.Activate(out bool isCreated);
            if (vfx == null) return null;
            if (isCreated)
            {

            }
            else
            {
                vfx.ResetStatus();
                vfx.gameObject.SetActive(true);
            }
            vfx.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, angle));
            return vfx;
        }

        public void Despawn(VFXObject vfxObject)
        {
            if (vfxObject == null) return;
            if (!_pools.TryGetValue(vfxObject.Name, out Pool<VFXObject> pool)) return;

            pool.Deactivate(vfxObject);
            vfxObject.gameObject.SetActive(false);
        }

        private void CreatePool(string name)
        {
            if (_vfxs == null) return;

            VFXObject vfxPrefab = _vfxs.Get(name);
            if (vfxPrefab == null) return;

            GameObject poolParent = new GameObject($"{name} Pool");
            poolParent.transform.SetParent(this.transform);

            Pool<VFXObject> pool = new()
            {
                Prefab = vfxPrefab,
                Parent = poolParent.transform,
                IsSetActive = false
            };
            _pools[name] = pool;
        }
    }
}
