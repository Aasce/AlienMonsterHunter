using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HorizontBreaker_Light_Ability : Ability, ISendDamageAbility
    {
        [Header("References")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LineRenderer _lineFOV;
        [SerializeField] private ParticleSystem _endLineParticle;

        [Header("Attack Settings")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Cooldown _dealDamageCooldown = new(0.25f);

        [Header("Runtime")]
        [SerializeField, Min(0f)] private float _width = 2f;
        [SerializeField, Min(0f)] private float _distance = 15f;
        [SerializeField] private Vector2 _direction;

        private readonly RaycastHit2D[] _raycastHitsCache = new RaycastHit2D[24];
        private ISendDamageable _ownerSender;

        private ContactFilter2D _filter;
        private Vector2 _boxSize;

        public event Action<DamageContainer> OnSendDamage;

        public float Damage => _damage;

        public override void Initialize()
        {
            base.Initialize();
            _width = Information.GetCustomValue("Width");
            _filter = new ContactFilter2D
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = _targetLayer
            };
        }

        public override void OnActive()
        {
            base.OnActive();
            _boxSize = new Vector2(_width, 0.1f);
            _ownerSender = Owner != null ? Owner.GetComponent<ISendDamageable>() : null;
            this.SetLine(startPosition: transform.position, _direction.normalized);
            this.SetLineWidth();
            this.Fire();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _width = Information.GetCustomValue("Width");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("Width", out LevelModification widthModification))
            {
                _width += widthModification.Value;
            }
        }

        private void Update()
        {
            if (Owner != null && Owner.activeInHierarchy) this.DespawnTime.Reset(); 
            else if (this.DespawnTime.CurrentTime > 1f) this.DespawnTime.CurrentTime = 0.1f;

            _dealDamageCooldown.Update(Time.deltaTime);
            if (_dealDamageCooldown.IsComplete)
            {
                this.Fire();
                _dealDamageCooldown.Reset();
            }
        }

        private void Fire()
        {
            int hitCount = Physics2D.BoxCast(
                transform.position,
                _boxSize,
                0f,
                _direction,
                _filter,
                _raycastHitsCache,
                _distance
            );

            for (int i = 0; i < hitCount; i++)
            {
                var hit = _raycastHitsCache[i];

                if (!hit.collider) continue;
                if (!hit.collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                this.SendDamage(target);
            }
        }

        private void SendDamage(ITargetable target)
        {
            DamageContainer container = new (_ownerSender, target as ITakeDamageable)
            {
                Damage = _damage
            };
            CombatController.Instance.DamageDealing(container);
            OnSendDamage?.Invoke(container);
        }

        public void Set(float damage, float distance, Vector2 direction)
        {
            _damage = damage;
            _distance = distance;
            _direction = direction.normalized;
        }


        private void SetLine(Vector2 startPosition, Vector2 direction)
        {
            Vector2 endPosition = startPosition + direction * _distance;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);

            _lineFOV.positionCount = 2;
            _lineFOV.SetPosition(0, startPosition);
            _lineFOV.SetPosition(1, endPosition);

            _endLineParticle.transform.position = endPosition;
        }

        private void SetLineWidth()
        {
            _lineRenderer.startWidth = _width * 1.25f;
            _lineRenderer.endWidth = _width * 1.5f;

            _lineFOV.startWidth = _width * 2.5f;
            _lineFOV.endWidth = _width * 2.5f;
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("DamageCooldown", _dealDamageCooldown.CurrentTime);
            data.SetCustom("Direction", _direction);
            data.SetCustom("Distance", _distance);
            data.SetCustom("Width", _width);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            _dealDamageCooldown.CurrentTime = data.GetCustom<float>("DamageCooldown");
            _direction = data.GetCustom<Vector2>("Direction");
            _distance = data.GetCustom<float>("Distance");
            _width = data.GetCustom<float>("Width");
            this.SetLineWidth();
        }

        protected override IEnumerator LoadOwner(AbilitySaveData data)
        {
            yield return base.LoadOwner(data);
            _ownerSender = Owner != null ? Owner.GetComponent<ISendDamageable>() : null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;
            Vector2 startPos = transform.position;
            Vector2 direction = _direction.normalized;
            float width = _width;
            float distance = _distance;
            
            Vector2 perp = Vector3.Cross(Vector3.forward, direction) * (width * 0.5f);

            Vector2 startA = startPos + perp;
            Vector2 startB = startPos - perp;
            Vector2 endA = startA + (direction * distance);
            Vector2 endB = startB + (direction * distance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(startA, endA);
            Gizmos.DrawLine(startB, endB);
            Gizmos.DrawLine(startA, startB);
            Gizmos.DrawLine(endA, endB);
        }
#endif

    }
}
