using Asce.Game.AIs;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Enemies
{
    [RequireComponent(typeof (NavMeshAgent))]
    public abstract class Enemy : Entity, IHasAgent, ISaveable<EnemySaveData>, ILoadable<EnemySaveData>
    {
        [Header("Enemy")]
        [SerializeField, Readonly] protected NavMeshAgent _agent;
        [SerializeField, Readonly] private SingleTargetDetection _targetDetection;

        [Header("Realtime")]
        [SerializeField, Readonly] protected Cooldown _attackCooldown = new();

        public new EnemyView View => base.View as EnemyView;
        public new EnemyStats Stats => base.Stats as EnemyStats;

        public NavMeshAgent Agent => _agent;
        public SingleTargetDetection TargetDetection => _targetDetection;
        public Cooldown AttackCooldown => _attackCooldown;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
        }

        public override void Initialize()
        {
            base.Initialize();
            Agent.speed = Stats.Speed.FinalValue;
            AttackCooldown.BaseTime = Stats.AttackSpeed.FinalValue;
            TargetDetection.Origin = transform;
            TargetDetection.ForwardReference = transform;
            TargetDetection.ViewRadius = Stats.ViewRange.FinalValue;

            Stats.Speed.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.speed = newValue;
            };

            Stats.ViewRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                TargetDetection.ViewRadius = newValue;
            };

            Stats.AttackSpeed.OnFinalValueChanged += (oldValue, newValue) =>
            {
                AttackCooldown.BaseTime = newValue;
            };

            this.OnDead += () =>
            {
                EnemyController.Instance.Despawn(this);
            };
        }


        protected virtual void Update()
        {
            TargetDetection.UpdateDetection();
            if (TargetDetection.CurrentTarget != null)
            {
                this.MoveToTaget();
            }
            
            this.AttackHandle();
            this.ViewHandle();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            AttackCooldown.Reset();

            View.ResetStatus();
            TargetDetection.ResetTarget();
        }

        protected abstract void MoveToTaget();
        protected abstract void Attack();

        protected virtual void AttackHandle()
        {
            AttackCooldown.Update(Time.deltaTime);
            if (AttackCooldown.IsComplete)
            {
                ITargetable target = TargetDetection.CurrentTarget;
                if (target == null) return;
                float attackRange = Stats.AttackRange.FinalValue;
                if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
                {
                    this.Attack();
                    AttackCooldown.Reset();
                }
            }
        }

        protected virtual void ViewHandle()
        {
            View.Animator.SetFloat("Speed", Agent.velocity.magnitude);
        }


        EnemySaveData ISaveable<EnemySaveData>.Save()
        {
            EntitySaveData baseData = ((ISaveable<EntitySaveData>)this).Save();
            EnemySaveData saveData = new();
            saveData.CopyFrom(baseData);
            saveData.attackCooldownRemaining = AttackCooldown.CurrentTime;
            OnBeforeSave(saveData);
            return saveData;
        }

        void ILoadable<EnemySaveData>.Load(EnemySaveData data)
        {
            ((ILoadable<EntitySaveData>)this).Load(data);
            Agent.Warp(data.position);
            AttackCooldown.CurrentTime = data.attackCooldownRemaining;
            OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(EnemySaveData data) { }
        protected virtual void OnAfterLoad(EnemySaveData data) { }
    }
}
