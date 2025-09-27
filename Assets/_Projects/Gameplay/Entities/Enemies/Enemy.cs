using Asce.Managers.Utils;
using System;
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

        [SerializeField] protected Cooldown _checkCooldown = new(2f);
        [SerializeField] protected LayerMask _seeLayer;

        [Space]
        [SerializeField] protected Cooldown _attackCooldown = new();

        public NavMeshAgent Agent => _agent;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
        }
        protected virtual void Start()
        {
            _agent.speed = 5f;// Speed.FinalValue;
            //Speed.OnFinalValueChanged += (oldVal, newVal) =>
            //{
            //    _agent.speed = newVal;
            //};

            //Health.OnCurrentValueChanged += (oldVal, newVal) =>
            //{
            //    if (newVal <= 0)
            //    {
            //        this.gameObject.SetActive(false);
            //    }
            //};
        }
        protected virtual void Update()
        {
            this.FindTargetHandle();
            this.AttackHandle();
        }

        protected abstract void MoveToTaget();
        protected abstract void FindTarget();
        protected abstract void Attack();

        protected virtual void FindTargetHandle()
        {
            if (_agent == null) return;

            _checkCooldown.Update(Time.deltaTime);
            if (_checkCooldown.IsComplete)
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
                _checkCooldown.Reset();
            }
        }
        protected virtual void AttackHandle()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (_attackCooldown.IsComplete && _target != null)
            {
                if (Vector2.Distance(transform.position, _target.transform.position) <= 1f) //_attackRange)
                {
                    this.Attack();
                    _attackCooldown.Reset();
                }
            }
        }

    }
}
