using UnityEngine;

namespace Asce.Game.VFXs
{
    public class VeylarExplosionSplatterVFXObject : ParticleVFXObject
    {
        [SerializeField] private ParticleSystem _mainSplatter;
        [SerializeField] private ParticleSystem _subSplatter;

        [Space]
        [SerializeField] private Vector2 _range = Vector2.one;

        [Header("Configs")]
        [SerializeField] private Vector2 _mainStartSizeForMaxSize;
        [SerializeField] private Vector2 _mainStartSizeForMinSize;
        [SerializeField] private Vector2 _mainShapeRadius;


        [Space]
        [SerializeField] private Vector2 _subStartSizeForMaxSize;
        [SerializeField] private Vector2 _subStartSizeForMinSize;
        [SerializeField] private Vector2 _subShapeRadius;

        public void SetSize(float size)
        {
            float t = Mathf.InverseLerp(_range.x, _range.y, size);
            this.SetMainSystem(t);
            this.SetSubSystem(t);
        }

        private void SetMainSystem(float t)
        {
            if (_mainSplatter == null) return;
            float min = Mathf.Lerp(_mainStartSizeForMinSize.x, _mainStartSizeForMaxSize.x, t);
            float max = Mathf.Lerp(_mainStartSizeForMinSize.y, _mainStartSizeForMaxSize.y, t);

            float shapeRadius = Mathf.Lerp(_mainShapeRadius.x, _mainShapeRadius.y, t);
            var mainModule = _mainSplatter.main;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(min, max);

            var shapeModule = _mainSplatter.shape;
            shapeModule.radius = shapeRadius;
        }

        private void SetSubSystem(float t)
        {
            if (_subSplatter == null) return;
            float min = Mathf.Lerp(_subStartSizeForMinSize.x, _subStartSizeForMaxSize.x, t);
            float max = Mathf.Lerp(_subStartSizeForMinSize.y, _subStartSizeForMaxSize.y, t);

            float shapeRadius = Mathf.Lerp(_subShapeRadius.x, _subShapeRadius.y, t);

            var mainModule = _subSplatter.main;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(min, max);

            var shapeModule = _subSplatter.shape;
            shapeModule.radius = shapeRadius;
        }

    }
}
