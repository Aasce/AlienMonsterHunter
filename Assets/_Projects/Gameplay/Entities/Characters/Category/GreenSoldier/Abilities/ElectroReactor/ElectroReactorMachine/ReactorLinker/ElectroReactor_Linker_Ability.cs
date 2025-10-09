using Asce.Game.Entities;
using Asce.Game.Entities.Machines;
using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ElectroReactor_Linker_Ability : Ability
    {
        [SerializeField] private Machine _startMachine;
        [SerializeField] private Machine _endMachine;

        [Space]
        [SerializeField, Min(0f)] private float _damageDeal = 0f;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField, Min(0f)] private float _linkWidth = 2f;
        [SerializeField, Min(0f)] private float _maxLinkDistance = 10f;
        [SerializeField] private Cooldown _shockCooldown = new(0.4f);

        [Space]
        [SerializeField] private LineRenderer _lineRenderer;

        public float DamageDeal
        {
            get => _damageDeal;
            set => _damageDeal = value;
        }

        public float LinkWidth
        {
            get => _linkWidth;
            set => _linkWidth = Mathf.Max(0f, value);
        }

        public float MaxLinkDistance
        {
            get => _maxLinkDistance;
            set => _maxLinkDistance = value;
        }

        private void Update()
        {
            if (_startMachine == null || !_startMachine.gameObject.activeInHierarchy) DespawnTime.ToComplete();
            else if (_endMachine == null || !_endMachine.gameObject.activeInHierarchy) DespawnTime.ToComplete();
            else if (Vector2.Distance(_startMachine.transform.position, _endMachine.transform.position) > MaxLinkDistance) DespawnTime.ToComplete();
            else DespawnTime.Reset();

            _shockCooldown.Update(Time.deltaTime);
            if (!_shockCooldown.IsComplete) return;
            _shockCooldown.Reset();
            this.Shocking();
        }

        private void LateUpdate()
        {
            this.UpdateLink();
        }

        public void Set(Machine start,  Machine end)
        {
            _startMachine = start;
            _endMachine = end;
            this.SetLinkLine();
        }

        private void Shocking()
        {
            if (_startMachine == null) return;
            if (_endMachine == null) return;

            Vector2 startPos = _startMachine.transform.position;
            Vector2 endPos = _endMachine.transform.position;
            Vector2 direction = (endPos - startPos).normalized;
            float distance = Vector2.Distance(startPos, endPos);

            // Cast a circle along the link line to detect targets
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                startPos,
                _linkWidth,
                direction,
                distance,
                _targetLayer
            );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) continue;
                if (!hit.collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                CombatController.Instance.DamageDealing(target as ITakeDamageable, DamageDeal);
            }
        }

        private void SetLinkLine()
        {
            if (_startMachine == null) return;
            if (_endMachine == null) return;

            Vector2 startPos = _startMachine.transform.position;
            Vector2 endPos = _endMachine.transform.position;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
            _lineRenderer.startWidth = _linkWidth * 3f;
            _lineRenderer.endWidth = _linkWidth * 3f;
        }

        private void UpdateLink()
        {
            if (_startMachine == null) return;
            if (_endMachine == null) return;

            Vector2 startPos = _startMachine.transform.position;
            Vector2 endPos = _endMachine.transform.position;

            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }
    }
}
