using Asce.Game.Levelings;
using Asce.Game.Progress;
using Asce.Core;
using Asce.Core.Attributes;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Guns
{
    [CreateAssetMenu(menuName = "Asce/Guns/Information", fileName = "Gun Information")]
    public class SO_GunInformation : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField, TextArea(3, 10)] private string _description = string.Empty;
        [SerializeField, SpritePreview] private Sprite _icon;
        [SerializeField] private GunType _type = GunType.Pistol;
        [SerializeField] private SO_LevelingInformation _leveling;
        [SerializeField] private SO_ProgressInformation _progress;

        [Header("Shoot")]
        [SerializeField, Min(0f)] private float _damage = 10f;
        [SerializeField, Min(0f)] private float _penetration = 0f;
        [SerializeField, Min(0f)] private float _shootSpeed = 0.5f;

        [Header("Magazine")]
        [SerializeField, Min(0)] private int _magazineSize = 10;
        [SerializeField, Min(0)] private int _startAmmo = 50;
        [SerializeField, Min(0f)] private float _reloadTime = 1f;

        [Header("Bullet Spread Settings")]
        [Tooltip("Minimum distance where bullet spread starts reducing.")]
        [SerializeField, Min(0f)] private float _minSpreadDistance = 3f;

        [Tooltip("Maximum distance where bullet spread is minimal.")]
        [SerializeField, Min(0f)] private float _maxSpreadDistance = 10f;

        [Tooltip("Maximum bullet spread angle (degrees) when distance is very close.")]
        [SerializeField, Range(0f, 30f)] private float _maxBulletSpreadAngle = 10f;

        [Tooltip("Minimum bullet spread angle (degrees) when distance is far.")]
        [SerializeField, Range(0f, 30f)] private float _minBulletSpreadAngle = 1f;

        [Space]
        [SerializeField]
        private ListObjects<string, CustomValue> _customValues = new((custom) =>
        {
            return custom.Name;
        });

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public GunType Type => _type;
        public SO_LevelingInformation Leveling => _leveling;
        public SO_ProgressInformation Progress => _progress;

        public float Damage => _damage;
        public float Penetration => _penetration;
        public float ShootSpeed => _shootSpeed;

        public int MagazineSize => _magazineSize;
        public int StartAmmo => _startAmmo;
        public float ReloadTime => _reloadTime;

        public float MinSpreadDistance => _minSpreadDistance;
        public float MaxSpreadDistance => _maxSpreadDistance;
        public float MaxBulletSpreadAngle => _maxBulletSpreadAngle;
        public float MinBulletSpreadAngle => _minBulletSpreadAngle;

        public ReadOnlyCollection<CustomValue> Customs => _customValues.List;
        public float GetCustomValue(string name)
        {
            if (_customValues.TryGet(name, out CustomValue customValue))
            {
                return customValue.Value;
            }

            Debug.LogWarning($"Custom Value \"{name}\" not found.", this);
            return 0f;
        }
    }
}
