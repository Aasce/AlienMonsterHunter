using UnityEditor;
using UnityEngine;

namespace Asce.Editors
{
    public class PolygonCollider2DScalerWindow : EditorWindow
    {
        private float _scaleFactor = 1f;
        private PolygonScaleCenterMode _centerMode = PolygonScaleCenterMode.TransformPosition;
        private bool _recenterToTransform = false;

        public enum PolygonScaleCenterMode
        {
            TransformPosition,
            VerticesCentroid
        }

        [MenuItem("Asce/Windows/Scale PolygonCollider2D")]
        private static void Open()
        {
            GetWindow<PolygonCollider2DScalerWindow>("Scale PolygonCollider2D");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Scale PolygonCollider2D", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _scaleFactor = EditorGUILayout.FloatField("Scale Factor", _scaleFactor);
            _centerMode = (PolygonScaleCenterMode)EditorGUILayout.EnumPopup("Center Mode", _centerMode);
            _recenterToTransform = EditorGUILayout.Toggle("Recenter To GameObject", _recenterToTransform);

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(_scaleFactor <= 0f))
            {
                if (GUILayout.Button("Apply"))
                {
                    ApplyScale(_scaleFactor, _centerMode, _recenterToTransform);
                }
            }
        }

        private static void ApplyScale(
            float scale,
            PolygonScaleCenterMode centerMode,
            bool recenter)
        {
            if (Selection.activeGameObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }

            PolygonCollider2D collider =
                Selection.activeGameObject.GetComponent<PolygonCollider2D>();

            if (collider == null)
            {
                Debug.LogWarning("Selected GameObject has no PolygonCollider2D.");
                return;
            }

            ScaleCollider(collider, scale, centerMode, recenter);
        }

        private static void ScaleCollider(
            PolygonCollider2D collider,
            float scale,
            PolygonScaleCenterMode centerMode,
            bool recenter)
        {
            Undo.RecordObject(collider, "Scale PolygonCollider2D");

            Vector2 scaleCenter = GetScaleCenter(collider, centerMode);

            for (int pathIndex = 0; pathIndex < collider.pathCount; pathIndex++)
            {
                Vector2[] points = collider.GetPath(pathIndex);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = scaleCenter + (points[i] - scaleCenter) * scale;
                }

                collider.SetPath(pathIndex, points);
            }

            if (recenter)
            {
                RecenterColliderToTransform(collider);
            }

            EditorUtility.SetDirty(collider);
        }

        private static Vector2 GetScaleCenter(
            PolygonCollider2D collider,
            PolygonScaleCenterMode mode)
        {
            switch (mode)
            {
                case PolygonScaleCenterMode.VerticesCentroid:
                    return CalculateVerticesCentroidLocal(collider);

                case PolygonScaleCenterMode.TransformPosition:
                default:
                    return Vector2.zero; // local origin
            }
        }

        private static Vector2 CalculateVerticesCentroidLocal(PolygonCollider2D collider)
        {
            Vector2 sum = Vector2.zero;
            int count = 0;

            for (int pathIndex = 0; pathIndex < collider.pathCount; pathIndex++)
            {
                Vector2[] points = collider.GetPath(pathIndex);

                for (int i = 0; i < points.Length; i++)
                {
                    sum += points[i];
                    count++;
                }
            }

            return count > 0 ? sum / count : Vector2.zero;
        }

        private static void RecenterColliderToTransform(PolygonCollider2D collider)
        {
            Vector2 centroid = CalculateVerticesCentroidLocal(collider);
            Vector2 offset = -centroid;

            for (int pathIndex = 0; pathIndex < collider.pathCount; pathIndex++)
            {
                Vector2[] points = collider.GetPath(pathIndex);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] += offset;
                }

                collider.SetPath(pathIndex, points);
            }
        }

    }
}
