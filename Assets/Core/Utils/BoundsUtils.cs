using UnityEngine;

namespace Asce.Managers.Utils
{
    public static class BoundsUtils 
    {
        /// <summary>
         ///    Gets the bounds of an orthographic camera. <br/>
         ///    If the camera is perspective, logs a warning and returns default.
         /// </summary>
        public static Bounds GetCameraBounds(Camera camera)
        {
            if (camera == null)
                return default;

            if (!camera.orthographic)
            {
                Debug.LogWarning("[CameraUtils] GetCameraBounds only supports Orthographic cameras.");
                return default;
            }

            float height = camera.orthographicSize * 2f;
            float width = height * camera.aspect;

            Vector3 center = camera.transform.position;
            center.z = 0; // For 2D games we ignore Z

            return new Bounds(center, new Vector3(width, height, 0));
        }
    }
}