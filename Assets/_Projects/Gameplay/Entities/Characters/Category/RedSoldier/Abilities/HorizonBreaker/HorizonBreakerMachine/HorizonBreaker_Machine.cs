using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    /// <summary>
    ///     A machine that fires a straight beam forward,
    ///     damaging all valid targets along its path.
    ///     When selected via OnGrimozSelected, draws the beam area preview.
    /// </summary>
    public class HorizonBreaker_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _selfFOV;
        [SerializeField] private Transform _barrel; // Optional fire start point

        [Space]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private string _lightAbilityName = string.Empty;

        [Header("Cooldown")]
        [SerializeField] private Cooldown _rechargeCooldown = new(2f);
        [SerializeField] private bool _fired = false;

        public Vector2 BarrielPosition => _barrel != null ? _barrel.position : transform.position;


        public override void Initialize()
        {
            base.Initialize();
            _rechargeCooldown.SetBaseTime(Information.Stats.GetCustomStat("RechargeTime"));
            _fired = false;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _rechargeCooldown.Reset();
            _fired = false;
        }
        
        private void Update()
        {
            if (_fired) return;
            _rechargeCooldown.Update(Time.deltaTime);
            if (_rechargeCooldown.IsComplete)
            {
                this.Fire();
                _fired = true;
            }
        }

        private void LateUpdate()
        {
            _selfFOV.DrawFieldOfView();
        }


        /// <summary>
        ///     Fires a straight beam in transform.up direction, damaging all valid targets hit.
        /// </summary>
        private void Fire()
        {
            HorizontBreaker_Light_Ability light = AbilityController.Instance.Spawn(_lightAbilityName, gameObject) as HorizontBreaker_Light_Ability;
            if (light == null) return;
            light.transform.position = BarrielPosition;
            light.Direction = transform.up;
            light.Damage = Information.Stats.GetCustomStat("Damage");
            light.Width = Information.Stats.GetCustomStat("Width");
            light.Distance = Information.Stats.GetCustomStat("Distance");
            light.gameObject.SetActive(true);
            light.OnActive();
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("AttackCooldown", _rechargeCooldown.CurrentTime);
            data.SetCustom("Fired", _fired);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _rechargeCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
            _fired = data.GetCustom<bool>("Fired");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Information == null) return;
            if (Information.Stats == null) return;

            float width = Information.Stats.GetCustomStat("Width");
            float distance = Information.Stats.GetCustomStat("Distance");

            Gizmos.color = Color.cyan;
            Vector2 direction = transform.up;
            Vector2 perp = Vector3.Cross(Vector3.forward, direction) * (width * 0.5f);

            Vector2 startA = BarrielPosition + perp;
            Vector2 startB = BarrielPosition - perp;
            Vector2 endA = startA + (direction * distance);
            Vector2 endB = startB + (direction * distance);

            Gizmos.DrawLine(startA, endA);
            Gizmos.DrawLine(startB, endB);
            Gizmos.DrawLine(startA, startB);
            Gizmos.DrawLine(endA, endB);
        }
#endif
    }
}
