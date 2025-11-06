using Asce.Game.Guns;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIGunInformation : UIObject
    {
        [SerializeField] private Image _gunIcon;
        [SerializeField] private Image _secondGunIcon;
        [SerializeField] private TextMeshProUGUI _magazineText;

        [Space]
        [SerializeField] private TextMeshProUGUI _reloadingAlertText;

        [Space]
        [SerializeField, Readonly] private Gun _gun;


        public Image GunIcon => _gunIcon;
        public Image SecondGunIcon => _secondGunIcon;
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
                if (GunIcon != null) GunIcon.sprite = null;
                if (MagazineText != null) MagazineText.text = "0/0";
                return;
            }

            this.SetGunIcon();
            this.SetGunMagazine();
            this.IsShowReloadAlert(Gun.IsReloading);

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

        private void SetGunIcon()
        {
            if (GunIcon == null) return;
            if (Gun.Information.Icon == null) return;

            Sprite icon = Gun.Information.Icon;
            float spriteWidth = icon.rect.width;
            float spriteHeight = icon.rect.height;
            float aspect = spriteHeight / spriteWidth;

            RectTransform rectTransform = GunIcon.rectTransform;
            float fixedWidth = rectTransform.sizeDelta.x;

            float newHeight = fixedWidth * aspect;
            rectTransform.sizeDelta = new Vector2(fixedWidth, newHeight);

            GunIcon.sprite = icon;
            GunIcon.preserveAspect = true;
        }

        private void SetGunMagazine()
        {
            if (MagazineText == null) return;
            MagazineText.text = $"{Gun.CurrentAmmo}/{Gun.RemainingAmmo}";
        }

        private void IsShowReloadAlert(bool isShow)
        {
            if (ReloadingAlertText == null) return;
            ReloadingAlertText.gameObject.SetActive(isShow);
        }

        private void Gun_OnCurrentAmmoChanged(float newAmmo) => this.SetGunMagazine();
        private void Gun_OnRemainingAmmoChanged(float newAmmo) => this.SetGunMagazine();

        private void Gun_OnStartReload() => this.IsShowReloadAlert(true);
        private void Gun_OnFinishReload() => this.IsShowReloadAlert(false);
    }
}
