using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.VFXs
{
    [RequireComponent(typeof(VFXObject))]
    public class ClearTrailVFXComponent : GameComponent
    {
        [SerializeField, Readonly] private VFXObject _vfx;
        [SerializeField] private List<TrailRenderer> _trails = new();

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _vfx);

            TrailRenderer[] renderers = transform.GetComponentsInChildren<TrailRenderer>();
            _trails.Clear();
            _trails.AddRange(renderers);
        }

        private void Start()
        {
            if (_vfx == null) return;
            _vfx.OnSpawn += VFX_OnSpawn;
        }

        private void VFX_OnSpawn()
        {
            this.StartCoroutine(this.ResetTrail());
        }

        public IEnumerator ResetTrail()
        {
            foreach (TrailRenderer trail in _trails)
            {
                if (trail == null) continue;
                trail.enabled = false;
            }

            yield return null;

            foreach (TrailRenderer trail in _trails)
            {
                if (trail == null) continue;
                trail.Clear();
                trail.enabled = true;
            }
        }
    }
}
