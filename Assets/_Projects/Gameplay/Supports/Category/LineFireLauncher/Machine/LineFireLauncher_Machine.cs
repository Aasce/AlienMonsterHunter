using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class LineFireLauncher_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private Transform _turret;
        [SerializeField] private Transform _barrelL;
        [SerializeField] private Transform _barrelR;

        [Header("Damage")]
        [SerializeField, Readonly] private float _damage = 10f;
        [SerializeField, Readonly] private int _numOfRocket = 3;

        [Header("Config")]
        [SerializeField] private string _missileAbilityName = "Oblivion Turret Bullet";
        [SerializeField] private Cooldown _fireCooldown = new(2f);
        [SerializeField] private float _fireAngle = 5f;
		
        [Header("Runtime")]
        [SerializeField, Readonly] private Vector2 _destination;
        [SerializeField, Readonly] private bool _isFiring = false;
        [SerializeField, Readonly] private int _rocketFiredCount = 0;

        public FieldOfView FovSelf => _fovSelf;
		
        public float Damage
        {
            get => _damage;
            protected set => _damage = value;
        }
		
        public Vector2 Destination
        {
            get => _destination;
            set => _destination = value;
        }
		
        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _fovSelf);
        }

        public override void Initialize()
        {
            base.Initialize();
            Effects.Untargetable.Add();
            Damage = Information.Stats.GetCustomStat("Damage");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Effects.Untargetable.Add();
            _rocketFiredCount = 0;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            Damage = Information.Stats.GetCustomStat("Damage");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("Damage", out LevelModification damageModification))
            {
                Damage += damageModification.Value;
            }
        }

        private void Update()
        {
            if (!_isFiring) return;
            _fireCooldown.Update(Time.deltaTime);
            if (!_fireCooldown.IsComplete) return;
            
            this.Fire();
            _rocketFiredCount++;
            _fireCooldown.Reset();

            if (_rocketFiredCount >= _numOfRocket)
            {
                _isFiring = false;
            }
        }

        private void LateUpdate()
        {
            _fovSelf.DrawFieldOfView();
        }

        public void StartFire()
        {
            _rocketFiredCount = 0;
            _isFiring = true;
        }
        private void Fire()
        {
            LineFireLauncher_Missile_Ability missile =
                AbilityController.Instance.Spawn(_missileAbilityName, gameObject) as LineFireLauncher_Missile_Ability;
            if (missile == null) return;

            // Base direction toward target
            Vector2 baseDir = (Destination - (Vector2)_turret.position).normalized;
            float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

            // Spread range: -half -> +half
            float minAngle = -_fireAngle * 0.5f;
            float maxAngle = _fireAngle * 0.5f;

            float t = (_numOfRocket <= 1) ? 0f : (float)_rocketFiredCount / (_numOfRocket - 1);
            float offsetAngle = Mathf.Lerp(minAngle, maxAngle, t);

            float finalAngle = baseAngle + offsetAngle;

            // Convert to direction
            float rad = finalAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            // Rotate turret to match this direction
            _turret.rotation = Quaternion.Euler(0f, 0f, finalAngle - 90f);
            Vector2 shootPos = GetShootPosition();

            missile.Leveling.SetLevel(Leveling.CurrentLevel);
            missile.Set(_damage, shootPos, direction);
            missile.gameObject.SetActive(true);
            missile.OnActive();
        }


        private Vector2 GetShootPosition()
        {
            if (_rocketFiredCount % 2 == 0) return _barrelL.position;
            else return _barrelR.position;
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("IsFiring", _isFiring);
            data.SetCustom("Damage", Damage);
            data.SetCustom("FireCooldown", _fireCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _isFiring = data.GetCustom<bool>("IsFiring");
            Damage = data.GetCustom<float>("Damage");
            _fireCooldown.CurrentTime = data.GetCustom<float>("FireCooldown");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
			
        }
#endif
    }
}
