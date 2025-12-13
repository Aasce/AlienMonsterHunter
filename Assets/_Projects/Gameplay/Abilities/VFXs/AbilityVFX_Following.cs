using Asce.Game.Abilities;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.VFXs
{
    [RequireComponent(typeof(Ability))]
    public class AbilityVFX_Following : GameComponent
    {
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField] private string _vfxName;
        [SerializeField] private float _despawnTimeAdding;
        [SerializeField] private bool _stopEmitOnAbilityDespawn = true;

        [Header("Runtime")]
        [SerializeField, Readonly] private VFXObject _followingVFX;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _ability);
        }


        private void Awake()
        {
            _ability.OnSpawn += Ability_OnSpawnEvent;
            _ability.OnDespawn += Ability_OnDespawnEvent;
        }

        private void Ability_OnSpawnEvent()
        {
            VFXObject vfx = VFXController.Instance.Spawn(_vfxName, transform.position, transform.eulerAngles.z);
            if (vfx == null) return;
            _followingVFX = vfx;
            _followingVFX.DespawnCooldown.SetBaseTime(_ability.DespawnTime.BaseTime + _despawnTimeAdding);

            if (!vfx.TryGetComponent(out FollowingVFXObject followingVFX)) return;
            followingVFX.Target = transform;
        }

        private void Ability_OnDespawnEvent()
        {
            if (_followingVFX == null) return;
            _followingVFX.DespawnCooldown.CurrentTime = _despawnTimeAdding;
            if (_stopEmitOnAbilityDespawn)
            {
                if (_followingVFX is ParticleVFXObject particleVFX)
                {
                    particleVFX.StopEmitting();
                }
            }
        }

    }
}
