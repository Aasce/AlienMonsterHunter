using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.VFXs
{
    [CreateAssetMenu(menuName = "Asce/VFXs/VFXs Data", fileName = "VFXs Data")]
    public class SO_VFXs : ScriptableObject
    {
        [SerializeField] private List<VFXObject> _vfxs = new();
        private ReadOnlyCollection<VFXObject> _vfxsReadonly;
        private Dictionary<string, VFXObject> _vfxsDictionary;

        public ReadOnlyCollection<VFXObject> VFXs => _vfxsReadonly ??= _vfxs.AsReadOnly();
        public VFXObject Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_vfxsDictionary == null) this.InitializeDictionary();
            _vfxsDictionary.TryGetValue(name, out VFXObject vfx);
            return vfx;
        }

        private void InitializeDictionary()
        {
            _vfxsDictionary = new Dictionary<string, VFXObject>();
            foreach (VFXObject vfx in _vfxs)
            {
                if (vfx == null) continue;
                if (_vfxsDictionary.ContainsKey(vfx.Name)) continue;
                _vfxsDictionary[vfx.Name] = vfx;
            }
        }
    }
}
