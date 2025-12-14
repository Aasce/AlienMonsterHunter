using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;
using System;

namespace Asce.Game.Entities.Machines
{
    /// <summary>
    ///     A machine that fires a straight beam forward,
    ///     damaging all valid targets along its path.
    ///     When selected via OnGrimozSelected, draws the beam area preview.
    /// </summary>
    public class HorizonBreaker_Machine : Machine, IMachineFireable
    {
        [Header("References")]
        [SerializeField] private FieldOfView _selfFOV;
        [SerializeField] private Transform _barrel; // Optional fire start point

        [Space]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private string _lightAbilityName = string.Empty;

        [Space]
        [SerializeField] private float _damage;
        [SerializeField] private float _distance;

        [Header("Cooldown")]
        [SerializeField] private Cooldown _rechargeCooldown = new(2f);
        [SerializeField] private bool _fired = false;

        public event Action<Vector2, Vector2> OnFired;

        public Vector2 BarrielPosition => _barrel != null ? _barrel.position : transform.position;


        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.Stats.GetCustomStat("Damage");
            _distance = Information.Stats.GetCustomStat("Distance");

            _rechargeCooldown.SetBaseTime(Information.Stats.GetCustomStat("RechargeTime"));
            _fired = false;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _rechargeCooldown.Reset();
            _fired = false;
        }
        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.Stats.GetCustomStat("Damage");
            _distance = Information.Stats.GetCustomStat("Distance");
            _rechargeCooldown.SetBaseTime(Information.Stats.GetCustomStat("RechargeTime"));
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            // Damage
            if (modificationGroup.TryGetModification("Damage", out LevelModification damageModification))
            {
                _damage += damageModification.Value;
            }

            // Distance
            if (modificationGroup.TryGetModification("Distance", out LevelModification distanceModification))
            {
                _distance += distanceModification.Value;
            }

            // RechargeTime
            if (modificationGroup.TryGetModification("RechargeTime", out LevelModification rechargeTimeModification))
            {
                _rechargeCooldown.BaseTime += rechargeTimeModification.Value;
                _rechargeCooldown.Reset();
            }
        }

        private void Update()
        {
            if (_fired) return;
            _rechargeCooldown.Update(Time.deltaTime);
            if (_rechargeCooldown.IsComplete)
            {
                this.Fire(BarrielPosition, transform.up);
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
        public void Fire(Vector2 position, Vector2 direction)
        {
            HorizontBreaker_Light_Ability light = AbilityController.Instance.Spawn(_lightAbilityName, gameObject) as HorizontBreaker_Light_Ability;
            if (light == null) return;
            light.transform.position = position;
            light.Set(_damage, _distance, direction);
            light.gameObject.SetActive(true);
            light.OnActive();

            OnFired?.Invoke(position, direction);
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Damage", _damage);
            data.SetCustom("Distance", _distance);
            data.SetCustom("RechargeSpeed", _rechargeCooldown.BaseTime);
            data.SetCustom("RechargeCooldown", _rechargeCooldown.CurrentTime);
            data.SetCustom("Fired", _fired);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _damage = data.GetCustom<float>("Damage");
            _distance = data.GetCustom<float>("Distance");
            _rechargeCooldown.BaseTime = data.GetCustom<float>("RechargeSpeed");
            _rechargeCooldown.CurrentTime = data.GetCustom<float>("RechargeCooldown");
            _fired = data.GetCustom<bool>("Fired");
        }
    }
}
