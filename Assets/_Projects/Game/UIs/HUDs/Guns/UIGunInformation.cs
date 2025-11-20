using Asce.Game.Guns;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs
{
    public class UIGunInformation : UIObject
    {
        [SerializeField] private Image _gunIcon;
        [SerializeField] private TextMeshProUGUI _magazineText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Space]
        [SerializeField] private TextMeshProUGUI _reloadingAlertText;

        [Space]
        [SerializeField, Readonly] private Gun _gun;


        public Image GunIcon => _gunIcon;
        public TextMeshProUGUI MagazineText => _magazineText;

        public TextMeshProUGUI ReloadingAlertText => _reloadingAlertText;

        public Gun Gun
        {
            get => _gun;
            set
            {
                if (_gun == value) return;
                this.UnregisterGun();
                _gun = value;
                this.RegisterGun();
            }
        }

        private void RegisterGun()
        {
            if (Gun == null || Gun.Information == null)
            {
                GunIcon.sprite = null;
                MagazineText.text = "0/0";
                return;
            }

            GunIcon.sprite = Gun.Information.Icon;
            this.SetGunMagazine();
            this.IsShowReloadAlert(Gun.IsReloading);
            _levelText.text = $"{Gun.Leveling.CurrentLevel}";

            Gun.Leveling.OnLevelChanged += Gun_OnLevelChanged;

            Gun.OnCurrentAmmoChanged += Gun_OnCurrentAmmoChanged;
            Gun.OnRemainingAmmoChanged += Gun_OnRemainingAmmoChanged;

            Gun.OnStartReload += Gun_OnStartReload;
            Gun.OnFinishReload += Gun_OnFinishReload;
        }

        private void UnregisterGun()
        {
            if (Gun == null) return;
            Gun.OnCurrentAmmoChanged -= Gun_OnCurrentAmmoChanged;
            Gun.OnRemainingAmmoChanged -= Gun_OnRemainingAmmoChanged;

            Gun.OnStartReload -= Gun_OnStartReload;
            Gun.OnFinishReload -= Gun_OnFinishReload;
        }

        private void SetGunMagazine() => MagazineText.text = $"{Gun.CurrentAmmo}/{Gun.RemainingAmmo}";
        private void IsShowReloadAlert(bool isShow) => ReloadingAlertText.gameObject.SetActive(isShow);


        private void Gun_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"{newLevel}";
        }

        private void Gun_OnCurrentAmmoChanged(float newAmmo) => this.SetGunMagazine();
        private void Gun_OnRemainingAmmoChanged(float newAmmo) => this.SetGunMagazine();

        private void Gun_OnStartReload() => this.IsShowReloadAlert(true);
        private void Gun_OnFinishReload() => this.IsShowReloadAlert(false);
    }
}
