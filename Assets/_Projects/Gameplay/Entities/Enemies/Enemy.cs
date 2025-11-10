using Asce.Game.AIs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
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

        public new SO_EnemyInformation Information => base.Information as SO_EnemyInformation;
        public new EnemyView View => base.View as EnemyView;
        public new EnemyStats Stats => base.Stats as EnemyStats;

        public NavMeshAgent Agent => _agent;
        public SingleTargetDetection TargetDetection => _targetDetection;
        public Cooldown AttackCooldown => _attackCooldown;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
            this.LoadComponent(out _targetDetection);
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

            Effects.Unmoveable.OnAffectChanged += (isAffect) =>
            {
                if (!isAffect) return;
                if (!Agent.isActiveAndEnabled) return;
                if (!Agent.isOnNavMesh) return;
                Agent.ResetPath();
            };

            this.OnDead += (Combats.DamageContainer container) =>
            {
                EnemyController.Instance.Despawn(this);
            };
        }


        protected virtual void Update()
        {
            TargetDetection.UpdateDetection();
            
            this.MoveHandle();
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
        
        protected virtual void MoveHandle()
        {
            if (TargetDetection.CurrentTarget == null) return;
            if (Effects.Unmoveable.IsAffect) return;

            this.MoveToTaget();
        }

        protected virtual void AttackHandle()
        {
            AttackCooldown.Update(Time.deltaTime);
            if (!AttackCooldown.IsComplete) return;

            if (Effects.Unattackable.IsAffect) return;

            ITargetable target = TargetDetection.CurrentTarget;
            if (target == null) return;

            float attackRange = Stats.AttackRange.FinalValue;

            Vector2 enemyPosition = transform.position;
            Vector2 targetPosition = target.transform.position;

            Vector2 targetCenter = targetPosition;
            float targetRadius = 0f;

            // Use collider info if available
            if (target.transform.TryGetComponent(out CircleCollider2D targetCollider))
            {
                targetCenter = (Vector2)targetPosition + targetCollider.offset;
                targetRadius = targetCollider.radius * target.transform.lossyScale.x;
            }

            Vector2 direction = targetCenter - enemyPosition;
            float distance = direction.magnitude;
            if (distance < Mathf.Epsilon) return;
            direction.Normalize();

            Vector2 closestPoint = targetCenter - direction * targetRadius;

            if (Vector2.Distance(enemyPosition, closestPoint) <= attackRange)
            {
                this.RotateToTarget();
                this.Attack();
                AttackCooldown.Reset();
            }
        }

        protected virtual void ViewHandle()
        {
            View.Animator.SetFloat("Speed", Agent.velocity.magnitude);

            if (Agent.velocity.magnitude > 0.02f)
            {
                float angle = Mathf.Atan2(Agent.velocity.y, Agent.velocity.x) * Mathf.Rad2Deg - 90f;
                float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z);
            }
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            Stats.AttackDamage.Clear(StatSourceType.Levelup);
            Stats.AttackSpeed.Clear(StatSourceType.Levelup);
            Stats.AttackRange.Clear(StatSourceType.Levelup);
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("AttackDamage", out LevelModification attackDamageModification))
            {
                Stats.AttackDamage.Add(attackDamageModification.Value, attackDamageModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("AttackSpeed", out LevelModification attackSpeedModification))
            {
                Stats.AttackSpeed.Add(attackSpeedModification.Value, attackSpeedModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("AttackRange", out LevelModification attackRangeModification))
            {
                Stats.AttackRange.Add(attackRangeModification.Value, attackRangeModification.Type.ToStatType(), StatSourceType.Levelup);
            }
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            float attackRange = Stats.AttackRange.FinalValue;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
#endif
    }
}
