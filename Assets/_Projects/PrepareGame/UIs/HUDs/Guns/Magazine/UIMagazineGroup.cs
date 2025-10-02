using Asce.Game.Guns;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIMagazineGroup : UIObject
    {
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _penetrationText;
        [SerializeField] private TextMeshProUGUI _fireRateText;

        [Space]
        [SerializeField] private TextMeshProUGUI _accurracyMinText;
        [SerializeField] private TextMeshProUGUI _accurracyMaxText;
        [SerializeField] private TextMeshProUGUI _maxSpreadText;

        [Space]
        [SerializeField] private TextMeshProUGUI _magazineSizeText;
        [SerializeField] private TextMeshProUGUI _startAmmoText;
        [SerializeField] private TextMeshProUGUI _reloadTimeText;


        public void Set(SO_GunInformation gunInformation)
        {
            if (gunInformation == null)
            {
                this.SetText(_damageText, $"NaN");
                this.SetText(_penetrationText, $"NaN");
                this.SetText(_fireRateText, $"NaN");

                this.SetText(_accurracyMinText, $"Min: NaN");
                this.SetText(_accurracyMaxText, $"Max: NaN");
                this.SetText(_maxSpreadText, $"NaN");

                this.SetText(_magazineSizeText, $"NaN");
                this.SetText(_startAmmoText, $"NaN");
                this.SetText(_reloadTimeText, $"NaN");
            }
            else
            {
                this.SetText(_damageText, $"{gunInformation.Damage}");
                this.SetText(_penetrationText, $"{0}");
                this.SetText(_fireRateText, $"{1f / gunInformation.ShootSpeed}/s");

                this.SetText(_accurracyMinText, $"Min: {0}");
                this.SetText(_accurracyMaxText, $"Max: {0}");
                this.SetText(_maxSpreadText, $"{0} deg");

                this.SetText(_magazineSizeText, $"{gunInformation.MagazineSize}");
                this.SetText(_startAmmoText, $"{gunInformation.StartAmmo}");
                this.SetText(_reloadTimeText, $"{gunInformation.ReloadTime}s");
            }
        }

        private void SetText(TextMeshProUGUI text, string value)
        {
            if (text == null) return;
            text.text = value;
        }
    }
}
