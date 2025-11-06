using log4net.Util;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public static class EnemyExtension
    {
        public static void DefaultMoveToTaget(this Enemy enemy)
        {
            if (enemy == null || enemy.TargetDetection == null) return;

            ITargetable target = enemy.TargetDetection.CurrentTarget;
            if (target == null) return;

            if (!target.transform.TryGetComponent(out CircleCollider2D targetCollider))
            {
                // Fallback: move directly to target position if no circle collider
                enemy.Agent.SetDestination(target.transform.position);
                return;
            }

            Vector2 enemyPosition = enemy.transform.position;
            Vector2 targetCenter = (Vector2)target.transform.position + targetCollider.offset;
            Vector2 direction = targetCenter - enemyPosition;

            // Inside Target
            if (direction.magnitude < targetCollider.radius) return;

            direction.Normalize();
            Vector2 closestPoint = targetCenter - direction * targetCollider.radius;

            float attackRange = enemy.Stats.AttackRange.FinalValue * 0.9f;
            Vector2 destination = closestPoint - direction * attackRange;

            enemy.Agent.SetDestination(destination);
        }


        public static void RotateToTarget(this Enemy enemy)
        {
            if (enemy == null) return;
            if (enemy.TargetDetection == null) return;

            ITargetable target = enemy.TargetDetection.CurrentTarget;
            if (target == null) return;

            Vector2 direction = target.transform.position - enemy.transform.position;
            float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}