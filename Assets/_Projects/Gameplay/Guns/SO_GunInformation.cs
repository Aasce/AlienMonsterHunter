using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Guns
{
    [CreateAssetMenu(menuName = "Asce/Guns/Information", fileName = "Gun Information")]
    public class SO_GunInformation : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField, TextArea(3, 10)] private string _description = string.Empty;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Header("Shoot")]
        [SerializeField, Min(0f)] private float _damage = 10f;
        [SerializeField, Min(0f)] private float _shootSpeed = 0.5f;

        [Header("Magazine")]
        [SerializeField, Min(0)] private int _magazineSize = 10;
        [SerializeField, Min(0)] private int _startAmmo = 50;
        [SerializeField, Min(0f)] private float _reloadTime = 1f;


        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

        public float Damage => _damage;
        public float ShootSpeed => _shootSpeed;

        public int MagazineSize => _magazineSize;
        public int StartAmmo => _startAmmo;
        public float ReloadTime => _reloadTime;
    }
}
