using Asce.Game.Abilities;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Shadowfang_Enemy : Enemy
    {
        [Header("Shadowfang Enemy")]
        [SerializeField] private Transform _mouth;

        protected override void MoveToTaget()
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = _target.transform.position;
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

        protected override void FindTarget()
        {
            this.DefaultFindTarget();
            if (Target != null) this.MoveToTaget();
        }

        protected override void Attack()
        {
            transform.up = _target.transform.position - transform.position;

            ShadowfangBullet_Ability bullet = AbilityController.Instance.Spawn("Shadowfang Bullet", gameObject) as ShadowfangBullet_Ability;
            if (bullet == null) return;

            Vector2 firePosition = _mouth != null ? _mouth.position : transform.position;
            Vector2 direction = (Vector2)_target.transform.position - firePosition;

            bullet.DamageDeal = Stats.AttackDamage.FinalValue;
            bullet.Fire(firePosition, direction);

        }
    }
}