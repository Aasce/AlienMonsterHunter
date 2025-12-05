using Asce.Core;
using UnityEngine;

namespace Asce.Game
{
    /// <summary>
    ///     Keeps this Transform unaffected by parent's scale.
    ///     The component restores the original local scale every LateUpdate.
    ///     <br/>
    ///     Useful for VFX, UI elements, or hitboxes that must not be distorted
    ///     when the parent GameObject changes scale.
    /// </summary>
    public class NonScaleTransform : GameComponent
    {
        [Header("Settings")]
        [SerializeField] private bool keepWorldScale = false;

        private Vector3 _originalLocalScale;
        private Vector3 _originalWorldScale;

        private void Awake()
        {
            _originalLocalScale = transform.localScale;
            _originalWorldScale = transform.lossyScale;
        }

        private void LateUpdate()
        {
            if (keepWorldScale) KeepWorldScale();
            else KeepLocalScale();
        }

        /// <summary>
        ///     Restores the original local scale (ignores parent's scaling).
        /// </summary>
        private void KeepLocalScale()
        {
            transform.localScale = _originalLocalScale;
        }

        /// <summary>
        ///     Restores original world scale by compensating parent scale distortion.
        /// </summary>
        private void KeepWorldScale()
        {
            if (transform.parent == null)
            {
                transform.localScale = _originalWorldScale;
                return;
            }

            Vector3 parentLossy = transform.parent.lossyScale;

            // Prevent division by zero
            parentLossy.x = parentLossy.x == 0 ? 1 : parentLossy.x;
            parentLossy.y = parentLossy.y == 0 ? 1 : parentLossy.y;
            parentLossy.z = parentLossy.z == 0 ? 1 : parentLossy.z;

            transform.localScale = new Vector3(
                _originalWorldScale.x / parentLossy.x,
                _originalWorldScale.y / parentLossy.y,
                _originalWorldScale.z / parentLossy.z
            );
        }
    }
}
