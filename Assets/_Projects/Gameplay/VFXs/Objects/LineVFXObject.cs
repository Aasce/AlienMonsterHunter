using UnityEngine;

namespace Asce.Game.VFXs
{
    public class LineVFXObject : VFXObject
    {
        [SerializeField] protected LineRenderer _lineRenderer;

        public LineRenderer LineRenderer => _lineRenderer;
    }
}
