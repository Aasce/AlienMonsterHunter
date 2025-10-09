using log4net.Util;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public static class EnemyExtension
    {
        public static void DefaultMoveToTaget(this Enemy enemy)
        {
            if (enemy == null) return;
            if (enemy.TargetDetection == null) return;

            ITargetable target = enemy.TargetDetection.CurrentTarget;
            if (target == null) return;

            enemy.Agent.SetDestination(target.transform.position);
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