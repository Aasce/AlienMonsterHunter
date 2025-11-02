using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
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

        [Header("Toxic Effects")]
        [SerializeField, Readonly] private float _toxicDuration = 0f;
        [SerializeField, Readonly] private float _toxicStrength = 0f;

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

        public override void Initialize()
        {
            base.Initialize();
            _toxicDuration = Information.GetCustomValue("ToxicDuration");
            _toxicStrength = Information.GetCustomValue("ToxicStrength");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _toxicDuration = Information.GetCustomValue("ToxicDuration");
            _toxicStrength = Information.GetCustomValue("ToxicStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ToxicDuration", out LevelModification toxicDurationModification))
            {
                _toxicDuration += toxicDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("ToxicStrength", out LevelModification toxicStrengthModification))
            {
                _toxicStrength += toxicStrengthModification.Value;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.IsDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _layer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (!target.IsTargetable) return;

            CombatController.Instance.DamageDealing(new DamageContainer(Owner.GetComponent<ISendDamageable>(), target as ITakeDamageable)
            {
                Damage = DamageDeal
            });
            EffectController.Instance.AddEffect("Toxic", Owner.GetComponent<Entity>(), target as Entity, new EffectData()
            {
                Duration = _toxicDuration,
                Strength = _toxicStrength,
            });

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

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);

            data.SetCustom("ToxicDuration", _toxicDuration);
            data.SetCustom("ToxicStrength", _toxicStrength);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _toxicDuration = data.GetCustom<float>("ToxicDuration");
            _toxicStrength = data.GetCustom<float>("ToxicStrength");
        }
    }
}