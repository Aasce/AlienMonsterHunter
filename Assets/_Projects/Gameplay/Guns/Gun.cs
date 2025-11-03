using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Guns
{
    public abstract class Gun : GameComponent, IIdentifiable, ISaveable<GunSaveData>, ILoadable<GunSaveData>
    {
        public const string PREFIX_ID = "gun";

        [SerializeField, Readonly] protected string _id;

        [Header("Setup")]
        [SerializeField] protected SO_GunInformation _information;
        [SerializeField, Readonly] protected Leveling _leveling;
        [SerializeField] protected Transform _barrel;
        [SerializeField] protected LayerMask _hitLayer;
        [SerializeField] protected IUsableGun _owner;

        [Header("Stats")]
        [SerializeField, Readonly] protected float _damage = 10f;
        [SerializeField, Readonly] protected float _penetration = 0f;
        [SerializeField, Readonly] protected float _shootSpeed = 1f;
        [SerializeField, Readonly] protected Cooldown _shootCooldown = new(0.5f);

        [Header("Magazine")]
        [SerializeField, Readonly] protected int _magazineSize = 10;
        [SerializeField, Readonly] protected int _startAmmo = 10;
        [SerializeField, Readonly] protected int _remainingAmmo;
        [SerializeField, Readonly] protected int _currentAmmo;

        [SerializeField] protected Cooldown _reloadCooldown = new(1.5f);

        [Header("Bullet Spread")]
        [SerializeField, Readonly] protected float _minSpreadDistance = 3f;
        [SerializeField, Readonly] protected float _maxSpreadDistance = 10f;
        [SerializeField, Readonly] protected float _maxBulletSpreadAngle = 10f;
        [SerializeField, Readonly] protected float _minBulletSpreadAngle = 1f;

        public event Action<float> OnRemainingAmmoChanged;
        public event Action<float> OnCurrentAmmoChanged;
        public event Action OnStartReload;
        public event Action OnFinishReload;
        public event Action<Vector2> OnFired;
        public event Action<Vector2, Vector2> OnHit;

        public string Id => _id;
        public SO_GunInformation Information => _information;
        public Leveling Leveling => _leveling;
        public virtual Vector2 BarrelPosition => _barrel != null ? _barrel.position : transform.position;
        public IUsableGun Owner
        {
            get => _owner;
            set => _owner = value;
        }
        public float Damage
        {
            get => _damage;
            protected set => _damage = value;
        }
        public float Penetration
        {
            get => _penetration;
            set => _penetration = value;
        }
        public float ShootSpeed
        {
            get => _shootSpeed;
            set => _shootSpeed = value;
        }
        public int MagazineSize
        {
            get => _magazineSize;
            protected set => _magazineSize = value;
        }
        public int RemainingAmmo
        {
            get => _remainingAmmo;
            set
            {
                _remainingAmmo = value;
                OnRemainingAmmoChanged?.Invoke(value);
            }
        }
        public int StartAmmo
        {
            get => _startAmmo; 
            set => _startAmmo = value;
        }
        public int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                _currentAmmo = value;
                OnCurrentAmmoChanged?.Invoke(value);
            }
        }

        public float MinSpreadDistance
        {
            get => _minSpreadDistance;
            set => _minSpreadDistance = value;
        }

        public float MaxSpreadDistance
        {
            get => _maxSpreadDistance;
            set => _maxSpreadDistance = value;
        }

        public float MaxBulletSpreadAngle
        {
            get => _maxBulletSpreadAngle;
            set => _maxBulletSpreadAngle = value;
        }

        public float MinBulletSpreadAngle
        {
            get => _minBulletSpreadAngle;
            set => _minBulletSpreadAngle = value;
        }

        public Cooldown ShootCooldown => _shootCooldown;
        public Cooldown ReloadCooldown => _reloadCooldown;

        public bool IsReloading => !_reloadCooldown.IsComplete;
        string IIdentifiable.Id
        {
            get => Id;
            set => _id = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _leveling);
        }

        public virtual void Initialize()
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
            Leveling.Initialize(Information.Leveling);

            Damage = Information.Damage;
            Penetration = Information.Penetration;
            ShootSpeed = Information.ShootSpeed;
            ShootCooldown.SetBaseTime(ShootSpeed, isReset: true);

            MagazineSize = Information.MagazineSize;
            StartAmmo = Information.StartAmmo;
            RemainingAmmo = StartAmmo;
            CurrentAmmo = MagazineSize;
            ReloadCooldown.SetBaseTime(Information.ReloadTime);
            ReloadCooldown.ToComplete();

            _minSpreadDistance = Information.MinSpreadDistance;
            _maxSpreadDistance = Information.MaxSpreadDistance;
            _maxBulletSpreadAngle = Information.MaxBulletSpreadAngle;
            _minBulletSpreadAngle = Information.MinBulletSpreadAngle;

            Leveling.OnLevelSetted += Leveling_OnLevelSetted;
            Leveling.OnLevelUp += Leveling_OnLevelUp;
        }

        public virtual void ResetStatus()
        {
            RemainingAmmo = StartAmmo;
            CurrentAmmo = MagazineSize;
            ReloadCooldown.ToComplete();
        }

        protected virtual void Update()
        {
            ShootCooldown.Update(Time.deltaTime);
            if (IsReloading)
            {
                ReloadCooldown.Update(Time.deltaTime);
                if (_reloadCooldown.IsComplete)
                {
                    this.ApplyReload();
                }
            }
        }

        public virtual void Fire(Vector2 direction)
        {
            if (IsReloading) return;
            if (CurrentAmmo > 0 && ShootCooldown.IsComplete)
            {
                // Apply bullet spread based on distance
                direction = ApplyBulletSpread(direction);

                this.Shooting(direction);
                this.OnFired?.Invoke(direction);
                ShootCooldown.Reset();
            }
        }

        public virtual void AltFire(Vector2 direction)
        {

        }

        public void Reload()
        {
            if (IsReloading) return;
            if (CurrentAmmo >= _magazineSize) return;
            if (RemainingAmmo <= 0) return;

            ReloadCooldown.Reset();
            OnStartReload?.Invoke();
        }

        /// <summary>
        ///     Apply a bullet spread based on distance between gun and target direction.
        /// </summary>
        protected virtual Vector2 ApplyBulletSpread(Vector2 direction)
        {
            float distance = direction.magnitude;
            if (distance <= 0.01f) return direction.normalized;

            // Calculate spread angle based on distance
            float spreadAngle;
            if (distance <= _minSpreadDistance)
                spreadAngle = _minBulletSpreadAngle;
            else if (distance >= _maxSpreadDistance)
                spreadAngle = _maxBulletSpreadAngle;
            else
            {
                float t = (distance - _minSpreadDistance) / (_maxSpreadDistance - _minSpreadDistance);
                spreadAngle = Mathf.Lerp(_minBulletSpreadAngle, _maxBulletSpreadAngle, t);
            }

            // Apply random rotation within spread range
            float randomAngle = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;

            return direction.normalized;
        }

        protected abstract void Shooting(Vector2 direction);

        protected virtual void ApplyReload()
        {
            int need = _magazineSize - CurrentAmmo;
            if (_remainingAmmo > need)
            {
                CurrentAmmo = _magazineSize;
                RemainingAmmo -= need;
            }
            else
            {
                CurrentAmmo += RemainingAmmo;
                RemainingAmmo = 0;
            }
            OnFinishReload?.Invoke();
        }

        protected virtual void Hit(Vector2 position, Vector2 direction)
        {
            OnHit?.Invoke(position, direction);
        }

        protected virtual void LevelTo(int newLevel)
        {
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("Damage", out LevelModification damageModification))
            {
                Damage += damageModification.Value;
            }

            if (modificationGroup.TryGetModification("Penetration", out LevelModification penetrationModification))
            {
                Penetration += penetrationModification.Value;
            }

            if (modificationGroup.TryGetModification("ShootSpeed", out LevelModification shootSpeedModification))
            {
                ShootSpeed += shootSpeedModification.Value;
                ShootCooldown.BaseTime = ShootSpeed;
                ShootCooldown.ToComplete();
            }

            if (modificationGroup.TryGetModification("MagazineSize", out LevelModification magazineSizeModification))
            {
                MagazineSize += (int)magazineSizeModification.Value;
                CurrentAmmo = MagazineSize;
            }

            if (modificationGroup.TryGetModification("StartAmmo", out LevelModification startAmmoModification))
            {
                StartAmmo += (int)startAmmoModification.Value;
                RemainingAmmo = StartAmmo;
            }

            if (modificationGroup.TryGetModification("ReloadCooldown", out LevelModification reloadCooldownModification))
            {
                ReloadCooldown.BaseTime += reloadCooldownModification.Value;
                ReloadCooldown.ToComplete();
            }
        }

        protected virtual void Leveling_OnLevelSetted(int newLevel)
        {
            Damage = Information.Damage;
            Penetration = Information.Penetration;
            ShootSpeed = Information.ShootSpeed;

            MagazineSize = Information.MagazineSize;
            StartAmmo = Information.StartAmmo;
            ReloadCooldown.SetBaseTime(Information.ReloadTime);
            ReloadCooldown.ToComplete();
            for (int i = 1; i <= newLevel; i++)
            {
                this.LevelTo(i);
            }
        }
        protected virtual void Leveling_OnLevelUp(int newLevel) => LevelTo(newLevel);

        GunSaveData ISaveable<GunSaveData>.Save()
        {
            GunSaveData saveData = new()
            {
                id = Id,
                name = Information.Name,
                damage = Damage,
                penetration = Penetration,
                shootSpeed = ShootSpeed,
                currentAmmo = CurrentAmmo,
                remainingAmmo = RemainingAmmo,
                magazineSize = MagazineSize,
                startAmmo = StartAmmo,
                reloadSpeed = ReloadCooldown.BaseTime,
            };

            this.OnBeforeSave(saveData);
            return saveData;
        }

        void ILoadable<GunSaveData>.Load(GunSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            Damage = data.damage;
            Penetration = data.penetration;
            ShootSpeed = data.shootSpeed;
            CurrentAmmo = data.currentAmmo;
            RemainingAmmo = data.remainingAmmo;
            MagazineSize = data.magazineSize;
            StartAmmo = data.startAmmo;
            ReloadCooldown.SetBaseTime(data.reloadSpeed);
            ReloadCooldown.ToComplete();

            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(GunSaveData data) { }
        protected virtual void OnAfterLoad(GunSaveData data) { }

#if UNITY_EDITOR
        // Place this method inside your Gun class
        protected virtual void OnDrawGizmosSelected()
        {
            // Barrel world position
            Vector3 origin = _barrel != null ? _barrel.position : transform.position;
            Vector3 forward = transform.up.normalized;
            if (forward.sqrMagnitude < 0.0001f) forward = Vector3.up;

            float minSpreadDistance = Application.isPlaying ? MinSpreadDistance : Information.MinSpreadDistance;
            float maxSpreadDistance = Application.isPlaying ? MaxSpreadDistance : Information.MaxSpreadDistance;
            float maxBulletSpreadAngle = Application.isPlaying ? MaxBulletSpreadAngle : Information.MaxBulletSpreadAngle;
            float minBulletSpreadAngle = Application.isPlaying ? MinBulletSpreadAngle : Information.MinBulletSpreadAngle;

            // Draw the two distance circles
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, minSpreadDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(origin, maxSpreadDistance);

            // Helper to draw spread edge lines for a given angle and distance
            void DrawSpreadEdges(float angleDegrees, float distance, Color col)
            {
                float angleLeft = -angleDegrees;
                float angleRight = angleDegrees;

                Vector3 leftDir = Quaternion.Euler(0f, 0f, angleLeft) * forward;
                Vector3 rightDir = Quaternion.Euler(0f, 0f, angleRight) * forward;

                Gizmos.color = col;
                Gizmos.DrawLine(origin, origin + leftDir * distance);
                Gizmos.DrawLine(origin, origin + rightDir * distance);
            }

            // Draw max spread edges to max distance (more visible)
            DrawSpreadEdges(maxBulletSpreadAngle, maxSpreadDistance, new Color(1f, 0.5f, 0.5f, 1f)); // reddish

            // Draw min spread edges to max distance (for comparison)
            DrawSpreadEdges(minBulletSpreadAngle, maxSpreadDistance, new Color(0.5f, 1f, 0.5f, 1f)); // greenish

            // Optionally draw short lines at min distance for the max angle (to show cone at minDist)
            DrawSpreadEdges(maxBulletSpreadAngle, minSpreadDistance, new Color(1f, 0.25f, 0.25f, 1f));
        }
#endif

    }
}
