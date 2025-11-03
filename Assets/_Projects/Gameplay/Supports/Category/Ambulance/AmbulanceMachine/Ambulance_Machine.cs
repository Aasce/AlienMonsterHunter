using Asce.Game.Combats;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Machines
{
    public class Ambulance_Machine : Machine, IHasAgent
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private ParticleSystem _healRadiusVFX;

        [Space]
        [SerializeField] private float _healAmount = 5f;
        [SerializeField] private float _healRadius = 5f;
        [SerializeField] private Cooldown _healCooldown = new(2f);
        [SerializeField] private LayerMask _healeeLayer;

        [Header("VFXs")]
        [SerializeField] private string _healVFXName = string.Empty;

        public NavMeshAgent Agent => _agent;

        public float HealRadius
        {
            get => _healRadius;
            protected set
            {
                _healRadius = value;
                var mainModule = _healRadiusVFX.main;
                mainModule.startSize = _healRadius * 2f; 
                _healRadiusVFX.Clear();
                _healRadiusVFX.Play();
                _fovSelf.ViewRadius = _healRadius + 1f;
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
        }

        public override void Initialize()
        {
            base.Initialize();
            _healAmount = Information.Stats.GetCustomStat("HealAmount");
            HealRadius = Information.Stats.GetCustomStat("HealRadius");
            _healCooldown.SetBaseTime(Information.Stats.GetCustomStat("HealInterval"));
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _healAmount = Information.Stats.GetCustomStat("HealAmount");
            HealRadius = Information.Stats.GetCustomStat("HealRadius");
            _healCooldown.SetBaseTime(Information.Stats.GetCustomStat("HealInterval"));
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("HealAmount", out LevelModification healAmountModification))
            {
                _healAmount += healAmountModification.Value;
            }

            if (modificationGroup.TryGetModification("HealRadius", out LevelModification healRadiusModification))
            {
                HealRadius += healRadiusModification.Value;
            }

            if (modificationGroup.TryGetModification("HealInterval", out LevelModification healIntervalModification))
            {
                _healCooldown.BaseTime += healIntervalModification.Value;
            }

        }

        private void Update()
        {
            _healCooldown.Update(Time.deltaTime);
            if (_healCooldown.IsComplete)
            {
                _healCooldown.Reset();
                this.Healing();
            }

            if (Agent.velocity.magnitude > 0.02f)
            {
                float angle = Mathf.Atan2(Agent.velocity.y, Agent.velocity.x) * Mathf.Rad2Deg - 90f;
                float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
            }
        }


        private void LateUpdate()
        {
            _fovSelf.DrawFieldOfView();
        }

        private void Healing()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, HealRadius, _healeeLayer);
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

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("HealAmount", _healAmount);
            data.SetCustom("HealRadius", HealRadius);
            data.SetCustom("HealInterval", _healCooldown.BaseTime);
            data.SetCustom("HealCooldown", _healCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            Agent.Warp(data.position);
            _healAmount = data.GetCustom<float>("HealAmount");
            HealRadius = data.GetCustom<float>("HealRadius");
            _healCooldown.BaseTime = data.GetCustom<float>("HealInterval");
            _healCooldown.CurrentTime = data.GetCustom<float>("HealCooldown");
        }
    }
}
