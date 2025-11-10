using Asce.Game.VFXs;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Effects
{
    [RequireComponent(typeof(Effect))]
    public class EffectVFX_FollowReceiver : GameComponent
    {
        [SerializeField, Readonly] private Effect _effect;
        [SerializeField] private string _vfxName;
        [SerializeField] private float _despawnTimeAdding = 0f;

        [Space]
        [SerializeField, Readonly] private VFXObject _vfxObject;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _effect);
        }

        private void Start()
        {
            this.Effect_OnApplied();
            _effect.OnApplied += Effect_OnApplied;
            _effect.OnUnapplied += Effect_OnUnapplied;
        }

        private void Effect_OnApplied()
        {
            if (_effect.Receiver == null) return;
            _vfxObject = VFXController.Instance.Spawn(_vfxName, _effect.Receiver.transform.position);
            if (_vfxObject == null) return;

            _vfxObject.DespawnCooldown.SetBaseTime(_effect.Duration.CurrentTime + _despawnTimeAdding);
            if (_vfxObject.TryGetComponent(out FollowingVFX following))
            {
                following.Target = _effect.Receiver.transform;
            }
        }

        private void Effect_OnUnapplied()
        {
            if (_vfxObject == null) return;

        }

    }
}