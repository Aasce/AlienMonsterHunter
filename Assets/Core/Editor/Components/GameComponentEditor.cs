using Asce.Core;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Asce.Editors
{
    [CustomEditor(typeof(GameComponent), editorForChildClasses: true)]
    public class GameComponentEditor : Editor
    {
        protected static readonly BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        #region - REF RESET -
        protected static MethodInfo _refResetMethod = typeof(GameComponent).GetMethod("RefReset", _bindingFlags);

        [MenuItem("CONTEXT/GameComponent/Ref Reset")]
        protected static void RefReset(MenuCommand command)
        {
            GameComponent gameComponent = (GameComponent)command.context;
            Undo.RecordObject(gameComponent, "Ref Reset");

            CallRefReset(gameComponent);

            EditorUtility.SetDirty(gameComponent);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameComponent.gameObject.scene);
        }

        protected static void CallRefReset(GameComponent gameComponent)
        {
            if (_refResetMethod != null)
                _refResetMethod.Invoke(gameComponent, null);
            else
                Debug.LogWarning($"Method 'RefReset' not found on {gameComponent.GetType().Name}");
        }
        #endregion

    }
}
