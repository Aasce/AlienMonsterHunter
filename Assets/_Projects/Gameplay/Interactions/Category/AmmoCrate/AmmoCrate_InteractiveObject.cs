using Asce.Game.Guns;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public class AmmoCrate_InteractiveObject : InteractiveObject
    {
        [Header("References")]
        [SerializeField, Readonly] private PolygonCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;

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
