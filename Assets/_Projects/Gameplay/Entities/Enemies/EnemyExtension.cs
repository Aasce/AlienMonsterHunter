using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public static class EnemyExtension
    {
        public static void DefaultMoveToTaget(this Enemy enemy)
        {
            if (enemy == null) return;
            if (enemy.Target == null) return;

            enemy.Agent.SetDestination(enemy.Target.transform.position);
        }

        public static void DefaultFindTarget(this Enemy enemy)
        {
            if (enemy == null) return;
            float viewRange = enemy.Stats.ViewRange.FinalValue;
            Vector2 position = enemy.transform.position;

            Collider2D[] characters = Physics2D.OverlapCircleAll(position, viewRange, enemy.TargetLayer);
            if (characters.Length == 0) return;

            foreach (Collider2D collder in characters)
            {
                Vector2 direction = (Vector2)collder.transform.position - position;
                RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, enemy.SeeLayer);
                if (hit.collider == null) continue;
                if (hit.collider.gameObject != collder.gameObject) continue;
                
                if (hit.transform.TryGetComponent(out Character character))
                {
                    enemy.Target = character;
                    break;
                }
            }
        }

    }
}