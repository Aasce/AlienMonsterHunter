using UnityEngine;
using UnityEditor;
using System.IO;

namespace Asce.Editors
{
    public static class CameraCapture
    {
        [MenuItem("Asce/Game Camera Screenshot")]
        public static void Capture()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogWarning("Camera.main not found.");
                return;
            }

            Vector2 gameViewSize = Handles.GetMainGameViewSize();
            int width = Mathf.RoundToInt(gameViewSize.x);
            int height = Mathf.RoundToInt(gameViewSize.y);

            string path = EditorUtility.SaveFilePanel(
                "Save Camera Screenshot",
                Application.dataPath,
                "CameraCapture",
                "png"
            );

            if (string.IsNullOrEmpty(path))
                return; // User cancelled

            RenderTexture rt = new RenderTexture(width, height, 24);
            RenderTexture prevRT = RenderTexture.active;

            cam.targetTexture = rt;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

            cam.Render();
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            cam.targetTexture = null;
            RenderTexture.active = prevRT;

            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);

            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);

            AssetDatabase.Refresh();

            Debug.Log($"Screenshot saved to: {path}");
        }
    }
}
