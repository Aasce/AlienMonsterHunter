using Asce.Game.Guns;
using Asce.Core.UIs;
using TMPro;
using UnityEngine;

namespace Asce.Game.UIs.Elements
{
    public class UIMagazineGroup : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _penetrationText;
        [SerializeField] private TextMeshProUGUI _fireRateText;

        [Space]
        [SerializeField] private TextMeshProUGUI _minSpreadDistanceText;
        [SerializeField] private TextMeshProUGUI _maxSpreadDistanceText;
        [SerializeField] private TextMeshProUGUI _minSpreadAngleText;
        [SerializeField] private TextMeshProUGUI _maxSpreadAngleText;

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

                this.SetText(_minSpreadDistanceText, $"NaN");
                this.SetText(_maxSpreadDistanceText, $"NaN");
                this.SetText(_minSpreadAngleText, $"NaN");
                this.SetText(_maxSpreadAngleText, $"NaN");

                this.SetText(_magazineSizeText, $"NaN");
                this.SetText(_startAmmoText, $"NaN");
                this.SetText(_reloadTimeText, $"NaN");
            }
            else
            {
                this.SetText(_damageText, $"{gunInformation.Damage}");
                this.SetText(_penetrationText, $"{gunInformation.Penetration}");
                this.SetText(_fireRateText, $"{1f / gunInformation.ShootSpeed:0.#}/s");

                this.SetText(_minSpreadDistanceText, $"{gunInformation.MinSpreadDistance:0.#}m");
                this.SetText(_maxSpreadDistanceText, $"{gunInformation.MaxSpreadDistance:0.#}m");
                this.SetText(_minSpreadAngleText, $"{gunInformation.MinBulletSpreadAngle:0.#} deg");
                this.SetText(_maxSpreadAngleText, $"{gunInformation.MaxBulletSpreadAngle:0.#} deg");

                this.SetText(_magazineSizeText, $"{gunInformation.MagazineSize:0.#}");
                this.SetText(_startAmmoText, $"{gunInformation.StartAmmo:0.#}");
                this.SetText(_reloadTimeText, $"{gunInformation.ReloadTime:0.#}s");
            }
        }

        private void SetText(TextMeshProUGUI text, string value)
        {
            if (text == null) return;
            text.text = value;
        }
    }
}
