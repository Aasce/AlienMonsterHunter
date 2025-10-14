using Asce.Game.FOVs;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Machines
{
    public class Ambulance_Machine : Machine, IHasAgent
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;

        [Space]
        [SerializeField] private float _healAmount = 5f;
        [SerializeField] private float _healRadius = 5f;
        [SerializeField] private Cooldown _healCooldown = new(2f);
        [SerializeField] private LayerMask _healeeLayer;

        [Header("VFXs")]
        [SerializeField] private string _healVFXName = string.Empty;

        public NavMeshAgent Agent => _agent;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
        }

        public override void Initialize()
        {
            base.Initialize();
            _healAmount = Information.Stats.GetCustomStat("HealAmount");
            _healRadius = Information.Stats.GetCustomStat("HealRadius");
            _healCooldown.SetBaseTime(Information.Stats.GetCustomStat("HealInterval"));
        }

        private void Update()
        {
            _healCooldown.Update(Time.deltaTime);
            if (_healCooldown.IsComplete)
            {
                _healCooldown.Reset();
                this.Healing();
            }
        }


        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
            _fovSelf.DrawFieldOfView();
        }

        private void Healing()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _healRadius, _healeeLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (collider.transform == transform) continue;
                if (!collider.TryGetComponent(out ITakeDamageable healee)) continue;
                if (healee is Machine) continue;

                CombatController.Instance.Healing(healee, _healAmount);
                this.SpawnHealVFX(collider.transform.position);
            }
        }

        private void SpawnHealVFX(Vector2 position)
        {
            VFXController.Instance.Spawn(_healVFXName, position);
        }
    }
}
