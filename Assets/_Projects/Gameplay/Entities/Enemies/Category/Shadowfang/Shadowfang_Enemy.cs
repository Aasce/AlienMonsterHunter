using Asce.Game.Abilities;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Shadowfang_Enemy : Enemy
    {
        [Header("Shadowfang Enemy")]
        [SerializeField] private Transform _mouth;

        protected override void MoveToTaget()
        {
            if (TargetDetection == null) return;
            ITargetable target = TargetDetection.CurrentTarget;
            if (target == null) return;

            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = target.transform.position;
            Vector2 direction = targetPosition - currentPosition;

            float distance = direction.magnitude;
            float attackRange = Stats.AttackRange.FinalValue * 0.9f;

            if (distance <= attackRange) // If already in attack range, do not move
            {
                _agent.ResetPath(); // stop moving
                return;
            }

            // Destination: stop at attack range from target
            Vector2 destination = targetPosition - direction.normalized * attackRange;
            _agent.SetDestination(destination);
        }

        protected override void Attack()
        {
            this.RotateToTarget();

            ShadowfangBullet_Ability bullet = AbilityController.Instance.Spawn("Shadowfang Bullet", gameObject) as ShadowfangBullet_Ability;
            if (bullet == null) return;

            Vector2 firePosition = _mouth != null ? _mouth.position : transform.position;
            Vector2 fireDirection = (Vector2)TargetDetection.CurrentTarget.transform.position - firePosition;

            bullet.DamageDeal = Stats.AttackDamage.FinalValue;
            bullet.Leveling.SetLevel(Leveling.CurrentLevel);
            bullet.gameObject.SetActive(true);
            bullet.Fire(firePosition, fireDirection);
            bullet.OnActive();
        }
    }
}