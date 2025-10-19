using Asce.Game.Entities;
using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class GunScope : GameComponent
    {
        [SerializeField] private Gun _gun;

        [Space]
        [SerializeField] private bool _isScope = false;

        [Space]
        [SerializeField] private Vector2 _spreadDistance;
        [SerializeField] private Vector2 _spreadAngle;

        [Space]
        [SerializeField] private float _addCameraSize;
        [SerializeField] private float _multiViewRadius;
        [SerializeField] private float _multiViewAngle;


        private float _cacheCameraOrthographicSize = 0;
        private string _cacheCharacterFovAngle;
        private string _cacheCharacterFovDistance;

        public Gun Gun 
        {
            get => _gun;
            set => _gun = value;
        }

        public void Toggle()
        {
            if (_isScope) this.CloseScope();
            else this.OpenScope();
        }

        public void OpenScope()
        {
            if (_isScope) return;
            _isScope = true;

            Gun.MinSpreadDistance = _spreadDistance.x;
            Gun.MaxSpreadDistance = _spreadDistance.y;
            Gun.MaxBulletSpreadAngle = _spreadAngle.x;
            Gun.MinBulletSpreadAngle = _spreadAngle.y;

            if (Gun.Owner.IsControlByPlayer())
            {
                _cacheCameraOrthographicSize = CameraController.Instance.MainCamera.orthographicSize;
                CameraController.Instance.MainCamera.orthographicSize += _addCameraSize;
            }
            if (Gun.Owner is Character character)
            {
                _cacheCharacterFovDistance = character.Stats.ViewRadius.Add(_multiViewRadius, Stats.StatValueType.Scale).Id;
                _cacheCharacterFovAngle = character.Stats.ViewAngle.Add(_multiViewAngle, Stats.StatValueType.Scale).Id;
            }
        }

        public void CloseScope()
        {
            if (!_isScope) return;
            _isScope = false;

            Gun.MinSpreadDistance = Gun.Information.MinSpreadDistance;
            Gun.MaxSpreadDistance = Gun.Information.MaxSpreadDistance;
            Gun.MaxBulletSpreadAngle = Gun.Information.MaxBulletSpreadAngle;
            Gun.MinBulletSpreadAngle = Gun.Information.MinBulletSpreadAngle;

            if (Gun.Owner.IsControlByPlayer())
            {
                CameraController.Instance.MainCamera.orthographicSize = _cacheCameraOrthographicSize;
            }
            if (Gun.Owner is Character character)
            {
                character.Stats.ViewRadius.RemoveById(_cacheCharacterFovDistance);
                character.Stats.ViewAngle.RemoveById(_cacheCharacterFovAngle);
            }
        }

    }
}
