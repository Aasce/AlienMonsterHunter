using Asce.Game.Stats;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class RedEyeLarval_Enemy : Enemy
    {

        protected override void Start()
        {
            base.Start();
            Agent.stoppingDistance = Stats.AttackRange.FinalValue * 0.9f;
            Stats.AttackRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.stoppingDistance = newValue;
            };
        }

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
            float damage = Stats.AttackDamage.FinalValue;
            CombatController.Instance.DamageDealing(_target, damage);
        }
    }
}
