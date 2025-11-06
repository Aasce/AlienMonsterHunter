using Asce.Game.Guns;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public class AmmoCrate_InteractiveObject : InteractiveObject, ISaveable<InteractiveObjectSaveData>
    {
        [Header("References")]
        [SerializeField, Readonly] private PolygonCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;
        bool ISaveable<InteractiveObjectSaveData>.IsNeedSave => false;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
        }

        public override void Interact(GameObject interacter)
        {
            if (!interacter.TryGetComponent(out IUsableGun gunUsable)) return;
            gunUsable.Gun.FillAmmo();
        }

    }
}
