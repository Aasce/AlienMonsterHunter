using Asce.Managers;
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

        [Header("Stats")]
        [SerializeField, Min(0f)] protected float _damage = 10f;
        [SerializeField] private Cooldown _shootCooldown = new(0.5f);

        [Header("Magazine")]
        [SerializeField, Min(1)] private int _magazineSize = 10;
        [SerializeField] private int _remainingAmmo;
        [SerializeField] private int _currentAmmo;

        [SerializeField] private Cooldown _reloadCooldown = new(1.5f);

        public event Action<float> OnRemainingAmmoChanged;
        public event Action<float> OnCurrentAmmoChanged;
        public event Action OnStartReload;
        public event Action OnFinishReload;


        public SO_GunInformation Information => _information;
        public Vector2 BarrelPosition => _barrel != null ? _barrel.position : transform.position;
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
        public Cooldown ShootCooldown => _shootCooldown;
        public Cooldown ReloadCooldown => _reloadCooldown;

        public bool IsReloading => !_reloadCooldown.IsComplete;

        /// <summary>
        ///     Must be called externally to initialize gun state.
        /// </summary>
        public virtual void Initialize()
        {
            if (Information == null) return;
            Damage = Information.Damage;
            ShootCooldown.SetBaseTime(Information.ShootSpeed, isReset: true);

            MagazineSize = Information.MagazineSize;
            RemainingAmmo = Information.StartAmmo;
            CurrentAmmo = MagazineSize;
            ReloadCooldown.SetBaseTime(Information.ReloadTime);
            ReloadCooldown.ToComplete(); // set as complete at start
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

        public virtual void Shoot(Vector2 direction)
        {
            if (IsReloading) return;
            if (CurrentAmmo > 0 && ShootCooldown.IsComplete)
            {
                this.Shooting(direction);
                ShootCooldown.Reset();
            }
        }

        public void Reload()
        {
            if (IsReloading) return;
            if (CurrentAmmo >= _magazineSize) return;
            if (RemainingAmmo <= 0) return;
            
            ReloadCooldown.Reset();
            OnStartReload?.Invoke();
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
    }
}
