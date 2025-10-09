using Asce.Game.Entities;
using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ShadowfangBullet_Ability : Ability
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        [Space]
        [SerializeField, Min(0f)] private float _damageDeal = 0f;
        [SerializeField] private bool _isDealing = false;

        [Space]
        [SerializeField] private LayerMask _layer;
        [SerializeField, Min(0f)] private float _force = 0f;

        public Rigidbody2D Rigidbody => _rigidbody;
        public float DamageDeal
        {
            get => _damageDeal;
            set => _damageDeal = value; 
        }

        public bool IsDealing
        {
            get => _isDealing;
            set => _isDealing = value;
        }
        public float Force
        {
            get => _force;
            set => _force = value;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.IsDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _layer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (!target.IsTargetable) return;
            
            CombatController.Instance.DamageDealing(target as ITakeDamageable, DamageDeal);

            this.IsDealing = true;
            this.DespawnTime.ToComplete();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            IsDealing = false;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            Rigidbody.gravityScale = 0f;
        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            if (Rigidbody == null) return;
            transform.position = position;
            Rigidbody.AddForce(direction.normalized * Force, ForceMode2D.Impulse);
            transform.up = direction;
        }
    }
}