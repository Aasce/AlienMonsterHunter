using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class RedEyeLarval_Enemy : Enemy
    {
        protected override void MoveToTaget()
        {
            _agent.SetDestination(_target.transform.position);
        }

        protected override void FindTarget()
        {
            List<Character> characters = ComponentUtils.FindAllComponentsInScene<Character>();
            if (characters.Count == 0) return;
            foreach (Character character in characters)
            {
                Vector2 direction = character.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, _seeLayer);
                if (hit.collider != null && hit.collider.gameObject == character.gameObject)
                {
                    _target = character;
                    this.MoveToTaget();
                    break;
                }
            }
        }

        protected override void Attack()
        {
            Debug.Log("Attack");
            // GameManager.Instance.CombatController.DamageDealing(_target, _attackDamage.FinalValue);
        }
    }
}
