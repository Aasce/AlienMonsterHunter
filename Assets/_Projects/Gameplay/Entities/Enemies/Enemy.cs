using Asce.Managers.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Enemies
{
    [RequireComponent(typeof (NavMeshAgent))]
    public abstract class Enemy : Entity
    {
        [Header("Enemy")]
        [SerializeField] protected NavMeshAgent _agent;

        [Header("Target Detection")]
        [SerializeField] protected Character _target;
        [SerializeField] protected LayerMask _targetLayer;

        [Space]
        [SerializeField] protected Cooldown _checkCooldown = new(2f);
        [SerializeField] protected LayerMask _seeLayer;

        [Space]
        [SerializeField] protected Cooldown _attackCooldown = new();

        public new EnemyView View => base.View as EnemyView;
        public new EnemyStats Stats => base.Stats as EnemyStats;

        public NavMeshAgent Agent => _agent;
        public Cooldown CheckCooldown => _checkCooldown;
        public Cooldown AttackCooldown => _attackCooldown;

        public LayerMask TargetLayer => _targetLayer;
        public LayerMask SeeLayer => _seeLayer;

        public Character Target
        {
            get => _target;
            set => _target = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
        }

        protected override void Start()
        {
            base.Start();
            Agent.speed = Stats.Speed.FinalValue;
            AttackCooldown.BaseTime = Stats.AttackSpeed.FinalValue;

            Stats.Speed.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.speed = newValue;
            };

            Stats.AttackSpeed.OnFinalValueChanged += (oldValue, newValue) =>
            {
                AttackCooldown.BaseTime = newValue;
            };

            Stats.Health.OnCurrentValueChanged += (oldValue, newValue) =>
            {
                if (newValue <= 0f) EnemyController.Instance.Despawn(this);
            };
        }


        protected virtual void Update()
        {
            this.FindTargetHandle();
            this.AttackHandle();
            this.ViewHandle();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            CheckCooldown.Reset();
            AttackCooldown.Reset();

            if (View != null) View.ResetStatus();
        }

        protected abstract void MoveToTaget();
        protected abstract void FindTarget();
        protected abstract void Attack();

        protected virtual void FindTargetHandle()
        {
            if (Agent == null) return;

            CheckCooldown.Update(Time.deltaTime);
            if (CheckCooldown.IsComplete)
            {
                if (_target != null)
                {
                    Vector2 direction = _target.transform.position - transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, _seeLayer);
                    if (hit.collider == null || hit.collider.gameObject != _target.gameObject)
                    {
                        _target = null;
                    }
                    else
                    {
                        this.MoveToTaget();
                    }
                }
                else
                {
                    this.FindTarget();
                }
                CheckCooldown.Reset();
            }
        }
        protected virtual void AttackHandle()
        {
            AttackCooldown.Update(Time.deltaTime);
            if (AttackCooldown.IsComplete && _target != null)
            {
                float attackRange = Stats.AttackRange.FinalValue;
                if (Vector2.Distance(transform.position, _target.transform.position) <= attackRange)
                {
                    this.Attack();
                    AttackCooldown.Reset();
                }
            }
        }

        protected virtual void ViewHandle()
        {
            if (View == null) return;
            View.Animator.SetFloat("Speed", Agent.velocity.magnitude);
        }
    }
}
