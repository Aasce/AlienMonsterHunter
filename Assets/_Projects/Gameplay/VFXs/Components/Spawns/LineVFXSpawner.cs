using Asce.Core;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class LineVFXSpawner : VFXSpawner
    {
        [Space]
        [SerializeField] private float _startWidth = 1f;
        [SerializeField] private float _endWidth = 1f;


        public override void Spawn(params Vector2[] points)
        {
            if (points.Length < 2) return; 
            LineVFXObject lineVFX = VFXController.Instance.Spawn(_vfxName, points[0]) as LineVFXObject;
            if (lineVFX == null) return;

            lineVFX.LineRenderer.positionCount = 2;
            lineVFX.LineRenderer.SetPosition(0, points[0]);
            lineVFX.LineRenderer.SetPosition(1, points[1]);
            lineVFX.LineRenderer.startWidth = _startWidth;
            lineVFX.LineRenderer.endWidth = _endWidth;
        }
    }
}