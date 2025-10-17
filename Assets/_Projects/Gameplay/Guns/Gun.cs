using Asce.Game.Entities;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Guns
{
    public abstract class Gun : GameComponent
    {
        [Header("Setup")]
        [SerializeField] protected SO_GunInformation _information;
        [SerializeField] protected Transform _barrel;
        [SerializeField] protected LayerMask _hitLayer;
        [SerializeField] protected Entity _owner;

        [Header("Stats")]
        [SerializeField, Readonly] protected float _damage = 10f;
        [SerializeField] protected Cooldown _shootCooldown = new(0.5f);

        [Header("Magazine")]
        [SerializeField, Readonly] protected int _magazineSize = 10;
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

        public SO_GunInformation Information => _information;
        public virtual Vector2 BarrelPosition => _barrel != null ? _barrel.position : transform.position;
        public Entity Owner
        {
            get => _owner;
            set => _owner = value;
        }
        public float Damage
        {
            get => _damage;
            protected set => _damage = value;
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

        public virtual void Initialize()
        {
            if (Information == null) return;
            Damage = Information.Damage;
            ShootCooldown.SetBaseTime(Information.ShootSpeed, isReset: true);

            MagazineSize = Information.MagazineSize;
            RemainingAmmo = Information.StartAmmo;
            CurrentAmmo = MagazineSize;
            ReloadCooldown.SetBaseTime(Information.ReloadTime);
            ReloadCooldown.ToComplete();

            _minSpreadDistance = Information.MinSpreadDistance;
            _maxSpreadDistance = Information.MaxSpreadDistance;
            _maxBulletSpreadAngle = Information.MaxBulletSpreadAngle;
            _minBulletSpreadAngle = Information.MinBulletSpreadAngle;
        }

        public virtual void ResetStatus()
        {
            Damage = Information.Damage;
            ShootCooldown.SetBaseTime(Information.ShootSpeed, isReset: true);

            MagazineSize = Information.MagazineSize;
            RemainingAmmo = Information.StartAmmo;
            CurrentAmmo = MagazineSize;
            ReloadCooldown.SetBaseTime(Information.ReloadTime);
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
